# Windows Web Forms demonstration
Build using Visual Studio Build Tools on Windows: `msbuild LegacyPortal.csproj /p:Configuration=Release`.
Host this directory as an IIS application with .NET Framework 4.8, HTTPS, Windows Authentication enabled and Anonymous Authentication disabled. Browse `/CaseDetail.aspx` as an authenticated Windows user. Verify initial data binding and postback filtering, header semantics, keyboard focus, and view-state validation.
This is a separate legacy host with synthetic records; it does not share the MVC authentication cookie or database. The Core API is a migration boundary, not an already implemented SSO integration. Windows runtime and JAWS verification must be performed on Windows.
