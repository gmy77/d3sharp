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

include "lib/LibMooege.php";

$email='';
$password='';

// See if request contains email & password.
if(array_key_exists('email', $_REQUEST))
    $email=$_REQUEST['email'];

if(array_key_exists('password', $_REQUEST))
    $password=$_REQUEST['password'];

if(array_key_exists('battleTag', $_REQUEST))
    $battleTag=$_REQUEST['battleTag'];

if(empty($email) || empty($password)) // if email or password is empty.
    print_account_form(); // print the login form.
else if(empty($battleTag))
    print_account_form();
else
    create_account($email, $password, $battleTag); // try loging using given credentals.

function create_account($email, $password, $battleTag)
{
    $mooege=new LibMooege("http://localhost", 9000); // change this line to match your configuration.
    
    if(!$mooege->connected) // check if we have a succesfull connection there.
        die("Can not connect to mooege!");
        
    $exists=$mooege->AccountExists($email);
    
    if($exists)
        die("An account already exists for email address: $email!");
    
    $created=$mooege->CreateAccount($email,$password,$battleTag);
    
    if($created)
        echo "Account created successfully!";
    else
        echo "Error creating account!";
}
?>

<? 
/**
 * Prints login form.
 */
function print_account_form() { ?>
<div style="width: 300px; border: 1px solid black; padding: 10px;">
    mooege login
    <form method="POST" action="register.php">
        E-mail:&nbsp&nbsp&nbsp&nbsp&nbsp <input type="text" name="email" size="16" value=""/><br />
        Password: <input type="password" name="password" size="16" /><br />
		BattleTag: <input type="text" name="battleTag" size="16" /><br />
        <div align="center">
            <p><input type="submit" value="Create" /></p>
        </div>
    </form>
</div>    
<? } ?>    