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

include "LibMooege.php";

/**
 * Only use this file for debugging LibMooege.php.
 */

PrintServicesDebugInfo();

/**
 * Prints services-info.
 */
function PrintServicesDebugInfo()
{
    $mooege = new LibMooege();
    $created=$mooege->CreateAccount("debug@","12345678");
    $exists=$mooege->AccountExists("debug@");
    $verified=$mooege->VerifyPassword("debug@","12345678");
?>
    <?if ($mooege->connected):?>
        Connected to <?=$mooege->servicesAddress?>.
        <ul>
            <li>Create account: debug@:12345678 [<?if($created):?>True<?else:?>False<?endif?>]</li>   
            <li>Account Exists: debug@ [<?if($exists):?>True<?else:?>False<?endif?>]</li>
            <li>Verify Password: 12345678 [<?if($verified):?>True<?else:?>False<?endif?>]</li>
        </ul>
    <?else:?>
        Not connected to mooege web-services!
    <?endif?>  
        
    <table border='1'><tr><th>Service</th><th>Query</th><th>Result</th></tr>        
        <tr><td>Core</td><td>Version</td><td><?=$mooege->Version()?></td></tr>
        <tr><td>Core</td><td>Uptime</td><td><?=$mooege->Uptime()?></td></tr>
        <tr><td>MooNet</td><td>IsMooNetServerOnline</td><td><?=$mooege->IsMooNetServerOnline()?></td></tr>
        <tr><td>MooNet</td><td>TotalAccounts</td><td><?=$mooege->TotalAccounts()?></td></tr>
        <tr><td>MooNet</td><td>TotalToons</td><td><?=$mooege->TotalToons()?></td></tr>
        <tr><td>MooNet</td><td>OnlinePlayerCount</td><td><?=$mooege->OnlinePlayerCount()?></td></tr>
        <tr><td>MooNet</td><td>OnlinePlayersList</td><td><?=print_r($mooege->OnlinePlayersList())?></td></tr>
        <tr><td>GameServer</td><td>IsGameServerOnline</td><td><?=$mooege->IsGameServerOnline()?></td></tr>
    </table>        
<?
}
?> 