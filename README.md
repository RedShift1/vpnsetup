# vpnsetup
Utility to create an L2TP/IPsec VPN with a PSK (Pre-Shared Key)

NOTE: This code is based on Cjwdev's work, see
https://blog.cjwdev.co.uk/2011/05/19/automate-creation-of-an-l2tp-vpn-with-pre-shared-key-and-automatically-use-windows-credentials/
There was no license posted with his code so I assumed public domain.

Usage: VpnSetup.exe -Name <String> -ServerAddress <String> -PresharedKey <String> [[-SplitTunneling]] [[-UseWinlogonCredential]]

# Examples

VpnSetup.exe -Name "New VPN" -ServerAddress vpn.mycompany.com -PresharedKey SomeSuperKey

Enable split tunneling (disable "Use default gateway on remote network" option)
VpnSetup.exe -Name "New VPN" -ServerAddress vpn.mycompany.com -PresharedKey SomeSuperKey -SplitTunneling

Enable split tunneling and use the Windows logon credentials
VpnSetup.exe -Name "New VPN" -ServerAddress vpn.mycompany.com -PresharedKey SomeSuperKey -SplitTunneling -UseWinlogonCredential
