1. Add latest mooege.exe to dependencies
2. Set output path to mooege output path (or it wont find sno aliases)
3. Run and Load file

If you cannot open a file it is probably because it is a netmon dump. Only libpcap/wireshark can be read.
To browse a netmon dump anyways you have to do the following:.

1. Open cap in wireshark
2. filter with "tcp.srcport == 1119 || tcp.dstport == 1119"
3. rightclick any packet. select "follow tcp stream"
4. make sure its not the bnet stream...if it is repeat step 2 and 3 with another packet selected (or change filter stream)
5. select hex dump view
6. select all, copy, paste into new file. make sure you have no empty lines at the end
7. file needs an .hex ending
8. open .hex file