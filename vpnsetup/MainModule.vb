Imports DotRas


Module MainModule

    Const EX_USAGE As Integer = 64

    ' Name of the VPN connection as it will appear in Windows
    Dim vpnName As String = ""
    ' Address to connect to
    Dim vpnServerAddress As String = ""
    ' L2TP pre-shared-key
    Dim vpnPSK As String = ""
    ' Enable split tunneling or not ("Use default gateway on remote network" option)
    Dim vpnUseRemoteGateway As Boolean = True
    ' "Automatically use my Windows logon name and password" option
    Dim vpnUseWinCreds As Boolean = False


    Sub Main()

        Dim PhoneBook As New RasPhoneBook

        parseCmdLine()
        verifyOptions()

        Try

            PhoneBook.Open()
            Dim VpnEntry As RasEntry = RasEntry.CreateVpnEntry(vpnName, vpnServerAddress, DotRas.RasVpnStrategy.L2tpOnly,
                                                                   DotRas.RasDevice.Create(vpnName, DotRas.RasDeviceType.Vpn))

            VpnEntry.Options.UsePreSharedKey = True
            VpnEntry.Options.RemoteDefaultGateway = vpnUseRemoteGateway
            VpnEntry.Options.UseLogOnCredentials = vpnUseWinCreds

            ' If this is set to True, it will enable CHAP and MS-CHAP v2
            ' without the option to override. So we disable this option
            ' and then set RequireMSChap2 to true to only allow MS-CHAP v2
            ' authentication. This does not mean the password will be sent
            ' in plaintext over the wire.
            VpnEntry.Options.RequireEncryptedPassword = False
            VpnEntry.Options.RequireMSChap2 = True

            PhoneBook.Entries.Add(VpnEntry)
            VpnEntry.UpdateCredentials(RasPreSharedKey.Client, vpnPSK)
            Console.WriteLine("VPN connection created successfully")
            Environment.Exit(0)

        Catch ex As Exception

            Console.WriteLine("ERROR: " & ex.Message & vbNewLine)
            Environment.Exit(999)

        End Try

    End Sub

    Private Sub ShowUsage()
        Console.WriteLine("Usage: VpnSetup.exe -Name <String> -ServerAddress <String> -PresharedKey <String> [[-SplitTunneling]] [[-UseWinlogonCredential]]" & vbNewLine & vbNewLine &
                          "Create a new L2TP VPN connection" & vbNewLine &
                          "EXAMPLE: VpnSetup.exe -Name ""New VPN"" -ServerAddress vpn.mycompany.com -PresharedKey SomeSuperKey -SplitTunneling" & vbNewLine)
    End Sub


    Sub parseCmdLine()
        Dim i As Integer = 0
        While i < My.Application.CommandLineArgs.Count
            Select Case My.Application.CommandLineArgs(i).ToLower
                Case "-name"
                    vpnName = My.Application.CommandLineArgs(i + 1)
                    i += 2
                Case "-serveraddress"
                    vpnServerAddress = My.Application.CommandLineArgs(i + 1)
                    i += 2
                Case "-presharedkey"
                    vpnPSK = My.Application.CommandLineArgs(i + 1)
                    i += 2
                Case "-splittunneling"
                    vpnUseRemoteGateway = False
                    i += 1
                Case "-usewinlogoncredential"
                    vpnUseWinCreds = True
                    i += 1
                Case Else
                    i += 1
            End Select
        End While
    End Sub


    Sub verifyOptions()
        If vpnName = "" Then
            wrErrWithUsageAndExit("-Name is required")
        End If
        If vpnServerAddress = "" Then
            wrErrWithUsageAndExit("-ServerAddress is required")
        End If
        If vpnPSK = "" Then
            wrErrWithUsageAndExit("-PresharedKey is required")
        End If
    End Sub


    Sub wrErrWithUsageAndExit(strErr As String)
        Console.WriteLine("Command line error: " & strErr & vbNewLine)
        ShowUsage()
        Environment.Exit(EX_USAGE)
    End Sub


End Module