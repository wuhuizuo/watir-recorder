; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{2B7401E8-7B9C-4B16-ADD3-E107DBAC7601}
AppName=WatirRecorderSharp
AppVersion=20101115
AppVerName=WatirRecorderSharp 20101201
AppPublisher=WatirBoy
AppPublisherURL=http://www.watirboy.com/
AppSupportURL=http://www.watirboy.com/
AppUpdatesURL=http://www.watirboy.com/
DefaultDirName={pf}\WatirRecorderSharp
DefaultGroupName=WatirRecorderSharp
AllowNoIcons=yes
LicenseFile=C:\code\watirrecordersharp\DotNet\README
OutputBaseFilename=WatirRecorderSharp
SetupIconFile=C:\code\watirrecordersharp\DotNet\src\app.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "C:\code\watirrecordersharp\DotNet\src\bin\Release\WatirRecorder.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\code\watirrecordersharp\DotNet\src\bin\Release\Interop.SHDocVw.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\code\watirrecordersharp\DotNet\src\bin\Release\SandBar.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\code\watirrecordersharp\DotNet\src\bin\Release\SandDock.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\code\watirrecordersharp\DotNet\src\bin\Release\config.xml"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\WatirRecorderSharp"; Filename: "{app}\WatirRecorder.exe"
Name: "{group}\{cm:UninstallProgram,WatirRecorderSharp}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\WatirRecorderSharp"; Filename: "{app}\WatirRecorder.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\WatirRecorderSharp"; Filename: "{app}\WatirRecorder.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\WatirRecorder.exe"; Description: "{cm:LaunchProgram,WatirRecorder#}"; Flags: nowait postinstall skipifsilent

