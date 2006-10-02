Imports Tva.Configuration.Common

Namespace ApplicationSecurity

    Partial Class SecurityProviderBase
        Inherits System.ComponentModel.Component

        <System.Diagnostics.DebuggerNonUserCode()> _
        Public Sub New(ByVal Container As System.ComponentModel.IContainer)
            MyClass.New()

            'Required for Windows.Forms Class Composition Designer support
            Container.Add(Me)

        End Sub

        <System.Diagnostics.DebuggerNonUserCode()> _
        Public Sub New()
            MyClass.New(SecurityServer.Development, "")
        End Sub

        'Component overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                Try
                    ' Save security database connection strings to the config file.
                    CategorizedSettings(ConfigurationElement).Add("Development", m_devConnectionString, _
                        "Connection string for connecting to development security database.", True)
                    CategorizedSettings(ConfigurationElement).Add("Acceptance", m_accConnectionString, _
                        "Connection string for connecting to acceptance security database.", True)
                    CategorizedSettings(ConfigurationElement).Add("Production", m_prdConnectionString, _
                        "Connection string for connecting to production security database.", True)
                    SaveSettings()
                Catch ex As Exception
                    ' We can safely ignore any exceptions encountered.
                End Try
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Component Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Component Designer
        'It can be modified using the Component Designer.
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            components = New System.ComponentModel.Container()
        End Sub

    End Class

End Namespace