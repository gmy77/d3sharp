<?php

/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

/**
 * LibMooege class that connect Mooege's web-services and communicate with them.
 * Requires php_soap extension to be activated.
 */
class LibMooege
{
    /** 
     * Is connected to mooege web-services?
     * @var type $connected
     */
    var $connected=false;
        
    /** 
     * Mooege web-services address
     * @var type $servicesAddress
     */
    var $servicesAddress;

    /** 
     * SOAP client for Core service.
     * @var type SOAPClient
     */
    var $core; 
    
    /** 
     * SOAP client for MooNet service.
     * @var type SOAPClient
     */
    var $moonet; 
    
    /**
     * SOAP client for GS service.
     * @var type $gs
     */
    var $gameserver;
    
    /**
     * SOAP client for accounts service
     * @var type $accounts.
     */
    var $accounts;
    
    /**
     * Creates a new instance of the LibMooege class.
     * @param type $address - Base address for mooege web-services.
     * @param type $port - Port for mooege web-services.
     * @param type $default_timeout - Default timeout value for connecting soap services.
     */
    public function __construct($address = "http://localhost", $port = 9000, $default_timeout = 5)
    {                
        $this->servicesAddress="$address:$port";
        $this->CreateSOAPClients($default_timeout);
    }
    
    /**
     * Creates soap-clients used for communicating mooege web-services.
     */
    private function CreateSOAPClients($timeout)
    {
        try 
        {          
            $this->core = new SoapClient($this->servicesAddress.'/Core?wsdl', array(
                'connection_timeout' => $timeout));
            
            $this->moonet = new SoapClient($this->servicesAddress.'/MooNet?wsdl', array(
                'connection_timeout' => $timeout));
            
            $this->gameserver = new SoapClient($this->servicesAddress.'/GS?wsdl', array(
                'connection_timeout' => $timeout));
            
            $this->accounts = new SoapClient($this->servicesAddress.'/Accounts?wsdl', array(
                'connection_timeout' => $timeout));
            
            $this->connected=true;
        }
        catch(Exception $e)
        {
            $this->connected=false;
        }
    }
    
    /**
     * Returns mooege version.
     * @return type string
     */
    public function Version()
    {
        if(!$this->connected)
            return "N/A";
        
        try {
            $response=$this->core->Version();
        }
        catch(Exception $e) {
            return "N/A";
        }
        
        return $response->VersionResult;     
    }
    /**
     * Returns uptime statistics for mooege.
     * @return type string
     */
    public function Uptime()
    {
        if(!$this->connected)
            return "N/A";
        
        try {
            $response=$this->core->Uptime();
        }
        catch(Exception $e) {
            return "N/A";
        }
        
        return $response->UptimeResult;     
    }
    
    /**
     * Returns true if MooNet is online.
     * @return type bool
     */
    public function IsMooNetServerOnline()
    {
        if(!$this->connected)
            return false;
        
        try {
            $response=$this->moonet->Ping();
        }
        catch(Exception $e) {
            return false;
        }
        
        return $response->PingResult;     
    }
    
    /**
     * Returns count of online players.
     * @return type int
     */
    public function OnlinePlayerCount()
    {
        if(!$this->connected)
            return -1;
        
        try {
            $response = $this->moonet->OnlinePlayersCount();
        }
        catch(Exception $e) {
            return -1;
        }
        
        return $response->OnlinePlayersCountResult;        
    }
    
    /**
     * Returns list of online players.
     * @return type array.
     */
    public function OnlinePlayersList()
    {
        if(!$this->connected)
            return array();
        
        try {
            $response = $this->moonet->OnlinePlayersList();
        }
        catch(Exception $e) {
            return array();
        }
                        
        if(property_exists($response->OnlinePlayersListResult, "string"))
            return $response->OnlinePlayersListResult->string;
        else
            return $response->OnlinePlayersListResult;
    }
    
    /**
    * Returns true if game-server is online.
    * @return type bool
    */
    public function IsGameServerOnline()
    {
        if(!$this->connected)
            return false;
        
        try {
            $response=$this->gameserver->Ping();        
        }
        catch(Exception $e) {
            return false;
        }
                
        return $response->PingResult;
    }
    
    /**
     * Creates a new account over mooege database.
     * Returns true if the call was successful, false otherwhise.
     * @param type $email
     * @param type $password
     * @return type bool
     */
    public function CreateAccount($email, $password, $battleTag)
    {
        if(!$this->connected)
            return false;
        
        try {
            $response=$this->accounts->CreateAccount(array('email' => $email, 'password' => $password, 'battleTag' => $battleTag));
        }
        catch(Exception $e) {
            return false;
        }
        
        return $response->CreateAccountResult;
    }
	
	public function ChangePassword($email, $password)
	{
	  if(!$this->connected)
		return false;			   

	  return $response=$this->accounts->ChangePassword(array('email' => $email, 'password' => $password));
	}
    
    /**
     * Returns true if an account exists for given email address, false otherwise.
     * @param type $email
     * @return type bool
     */
    public function AccountExists($email)
    {
        if(!$this->connected)
            return false;
        
        try {
            $response=$this->accounts->AccountExists(array('email' => $email));
        }
        catch(Exception $e) {
            return false;
        }
        
        return $response->AccountExistsResult;
    }
    
    /**
     * Returns true if password is correct, false otherwise.
     * @param type $email
     * @param type $password
     * @return type bool
     */
    public function VerifyPassword($email, $password)
    {
        if(!$this->connected)
            return false;
        
        try {
            $response=$this->accounts->VerifyPassword(array('email' => $email, 'password' => $password));
        }
        catch(Exception $e) {
            return false;
        }
        
        return $response->VerifyPasswordResult;
    }
    
    /**
     * Returns count of total accounts.
     * @return type int
     */
    public function TotalAccounts()
    {
        if(!$this->connected)
            return -1;
        
        try {
            $response = $this->accounts->TotalAccounts();
        }
        catch(Exception $e) {
            return -1;
        }
        
        return $response->TotalAccountsResult;        
    }
    
    /**
     * Returns count of total toons.
     * @return type int
     */
    public function TotalToons()
    {
        if(!$this->connected)
            return -1;
        
        try {
            $response = $this->accounts->TotalToons();
        }
        catch(Exception $e) {
            return -1;
        }
        
        return $response->TotalToonsResult;        
    }
}
?>