TabPlay ReadMe file

TabPlay is a system for playing the card game bridge using hand-held devices 
(tablets, phones, etc) over a local wireless network. It is a web application that uses 
Windows IIS to serve web pages across a local wireless network.  It requires 
a server PC and some sort of device with a web browser for each player.

TabPlay uses a Bridgemate .bws standard Access database, so is a direct 
replacement for BridgeTab (or Bridgemate, BridgePad etc).  It should work 
with any bridge scoring program that can create a .bws database, but has been 
built with EBUScore and Jeff Smith's scoring programs in mind.

TabPlay is designed for use on a PC with Windows 10 (which includes Internet
Information Services (IIS) 10), .NET Framework 4.7.2 and ASP.NET 4.7.

IMPORTANT: Please ensure you have completed all the installation steps in the 
'TabPlay User Guide' document before attempting to run TabPlay.

To upgrade TabPlay from a previous installation, please read the section in
the User Guide on Upgrading TabPlay.

TabPlay is currently limited to 4 sections (A, B, C and D in that order) and 
30 tables per section.  It can be used for pairs, teams, or Swiss events; it 
can also be used for individual events with EBUScore.

TabPlay implements a range of display options which can be set by the
scoring program, or from TabPlayStarter.  See the User Guide for more
details.

TabPlayStarter uses Bo Haglund's Double Dummy Solver (DDS) to analyse 
hand records.  DDS requires the Microsoft Visual C++ Redistributable (x86) 
2015 (or later) to be installed on the PC.

See the NOTICE and LICENSE files.