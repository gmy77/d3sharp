/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PonyLib;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PonyPatcher")]
[assembly: AssemblyDescription("mooege - an educational game server emulator.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("mooege.org")]
[assembly: AssemblyProduct("PonyPatcher")]
[assembly: AssemblyCopyright("Copyright © 2011 - 2012, mooege project.")]
[assembly: AssemblyTrademark("PonyPatcher")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("52ca2c50-25ca-4120-a951-f98f5fdddd5d")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

// Set the assembly version from VersionInfo.cs file.
[assembly: AssemblyVersion(VersionInfo.Assembly.Version)]
