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

$mooege = new LibMooege();

$count=$mooege->OnlinePlayerCount();
print_r($count);

$players=$mooege->OnlinePlayersList();
print_r($players);

class LibMooege
{
    var $connected=false;
    var $servicesAddress; // web-services address..
    
    var $moonetSOAPClient; // SOAP client for MooNet service.
    var $gsSOAPClient; // SOAP client for GS service.
    var $accountsSOAPClient; // SOAP client for accounts service.    
    
    public function __construct($address = "http://localhost", $port = 9000)
    {                
        $this->serviceAddress="$address:$port";
        $this->CreateSOAPClients();
    }
    
    private function CreateSOAPClients()
    {
        try {
            $this->moonetSOAPClient = new SoapClient($this->serviceAddress.'/MooNet?wsdl');
            $this->gsSOAPClient = new SoapClient($this->serviceAddress.'/GS?wsdl');
            $this->accountsSOAPClient = new SoapClient($this->serviceAddress.'/Accounts?wsdl');
            $this->connected=true;
        }
        catch(Exception $e)
        {
            echo $e;
            $this->connected=false;
        }
    }
    
    public function OnlinePlayerCount()
    {
        return $this->moonetSOAPClient->OnlinePlayerCount();
    }
    
    public function OnlinePlayersList()
    {
        return $this->moonetSOAPClient->OnlinePlayersList();
    }
}
?> 