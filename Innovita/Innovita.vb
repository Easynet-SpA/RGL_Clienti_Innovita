Imports RglFwk
'------------------------------------------------------------------------------------------
'------------------------------------------------------------------------------------------
Public Class Innovita
    Inherits SBOBase.SBOModule

    '----------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------
    Public Shared Sub SetConnectionString()

        '--------------------------------------
        ' SAP
        '--------------------------------------
        Dim cs As String = RglFwk.SBOBase.Util.GetSQLConnectionString

        If Rgl.Innovita.My.MySettings.Default.SAPConnectionString <> cs Then
            Rgl.Innovita.My.MySettings.Default.PropertyValues("SAPConnectionString").PropertyValue = cs
        End If

    End Sub
    '----------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------
    Public Overrides Sub SetParent(ByRef p As SBOBase.SBOAddOn)
        MyBase.SetParent(p)

        Rgl.Innovita.Config.Init(parent.GetConfigTable)

    End Sub
    '----------------------
    ' @return nome addon
    '----------------------
    Public Overrides Function GetModuleName() As String
        Return Me.GetType().Name
    End Function
    '-----------------------
    ' @return installer
    '-----------------------
    Public Overrides Function GetInstaller() As SBOBase.SBOInstallUserTable
        Return New InstallUserTable(parent, Me)
    End Function

    '--------------------------------
    ' Inizializza i menu e i form
    '--------------------------------
    Public Overrides Function Init() As Boolean
        Dim ret As Boolean = True
        Dim sboMenu As SBOBase.SBOMenu


        SetConnectionString()


        Try

            parent.LoadXMLMenuFromConfig("Rgl.Innovita.menu.xml", Me.GetType, New String() {"Rgl.Innovita.logo.png"})

            sboMenu = New SBOBase.GestioneConfigurazioneMenu(Me.parent)
            sboMenu.Attach("RGL_INNO_01")

            ''---------------------------------------------------------------------------------------------
            ''  Creazione etichette articoli
            ''---------------------------------------------------------------------------------------------
            'SBOBase.SBOMenuForm.NewMenu(parent, "RGL_PUR_03", GetType(Etichette.EtichettaArticoloForm))

            '---------------------------------------------------------------------------------------------
            ' Articoli e Bom
            '---------------------------------------------------------------------------------------------

            parent.InstallSystemForm(SBOBase.SBODefaultSystemFormHandler.NewHandler(parent, SBOBase.SBOConstant.ARTICOLI.FormType, GetType(AnagraficheSAP.ArticoliSystemForm)))

            parent.InstallSystemForm(SBOBase.SBODefaultSystemFormHandler.NewHandler(parent, SBOBase.SBOConstant.DISTINTA_BASE.FormType, GetType(AnagraficheSAP.DistintaBaseSystemForm)))
            parent.InstallSystemForm(SBOBase.SBODefaultSystemFormHandler.NewHandler(parent, 1880000003, GetType(AnagraficheSAP.GestioneDistintaBaseSystemForm)))


            ''---------------------------------------------------------------------------------------------
            ''
            ''---------------------------------------------------------------------------------------------
            'parent.InstallSystemForm(SBOBase.SBODefaultSystemFormHandler.NewHandler(parent, SBOBase.SBOConstant.ORDINE_ACQUISTO.FormType, GetType(DocumentiSAP.OrdineAcquistoSystemForm)))

            If parent.GetAppl.Company.UserName = "manager" Then
                '--------------------------------------------------------------
                ' Utility
                '--------------------------------------------------------------
                'sboMenu = New Utility.EstraiXmlUDOFormMenu(parent, "RGL_Rgl.Innovita_04")
                'sboMenu = New Utility.ImportaXmlUDOFormMenu(parent)
                sboMenu = New Utility.ImportaLayoutMenu(parent)
            End If


            Helper.InitIcone()

        Catch ex As Exception
            SBOBase.Util.Log(ex)
            ret = False
        End Try

        Return ret
    End Function
    '-----------------------------
    ' @return versione maggiore
    '-----------------------------
    Public Overrides Function GetMajorVersion() As Integer
        Return RglFwk.SBOBase.Util.GetAssemblyMajorVersion(GetType(Innovita))
    End Function
    '-----------------------------
    ' @return versione minore
    '-----------------------------
    Public Overrides Function GetMinorVersion() As Integer
        Return RglFwk.SBOBase.Util.GetAssemblyMinorVersion(GetType(Innovita))
    End Function

End Class
