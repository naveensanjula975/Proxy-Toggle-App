# Preliminary Design Review (PDR)

## 1. Introduction
- **Project Name:** Proxy Toggle App  
- **Version:** 0.1  

## 2. Objectives
- Provide seamless enable/disable proxy toggle  
- Support Git, npm, environment variables, and Windows system proxy  
- Intuitive UI with Fluent design  
- Single-file deployable `.exe`

## 3. System Overview
- **Platform:** Windows 10+  
- **Framework:** WPF (.NET 7) / WinUI 3  
- **Packaging:** Single-file self-contained EXE  

## 4. Requirements

### 4.1 Functional
- F1: Enable proxy (registry, Git, npm, ENV)  
- F2: Disable proxy (registry, Git, npm, ENV)  
- F3: Display current status  
- F4: Show progress steps and result notifications  

### 4.2 Non-Functional
- NF1: Startup < 1s  
- NF2: Memory footprint < 100 MB  
- NF3: UI responsiveness (async operations)  
- NF4: Light/dark theme support  

## 5. Architecture

```
+-------------------+     +---------------------+
|    UI Layer       | <-> | ViewModel/Commands  |
+-------------------+     +---------------------+
          |                         |
          v                         v
+--------------------------------+ +------------------+
| ProxyService (Registry, Env)  | | GitNpmService    |
+--------------------------------+ +------------------+
```

## 6. UI/UX Design

### 6.1 Main Window
- Title bar with icon and "Proxy Toggle"  
- Buttons: Enable Proxy, Disable Proxy, Exit  
- Status text: "Proxy Enabled/Disabled"  
- Progress indicator (spinner / progress bar)

### 6.2 Notifications
- Modal confirmation (optional)  
- In-app toast: success / error  

## 7. Data Flow

```
Start
  |
  v
User clicks Enable
  |
  v
Run Registry edits
  |
  v
Run Git/npm commands
  |
  v
Set ENV vars
  |
  v
Update UI status & notify
  |
  v
Error? --> Yes --> Rollback on error --> Exit
  |
  v
 No
  |
  v
Exit
```

## 8. Error Handling
- Catch exceptions in each step
- Aggregate errors into a log
- Show error dialog with "Copy log"

## 9. Test Plan
- **Unit Tests:** RegistryService, GitNpmService
- **Integration Tests:** Full enable/disable flow
- **UI Tests:** Button clicks, notifications

## 10. Schedule

| Task                    | Duration | Start      | End        |
|-------------------------|----------|------------|------------|
| Architecture & PDR      | 2 days   | 2025-07-08 | 2025-07-09 |
| UI Prototyping          | 3 days   | 2025-07-10 | 2025-07-14 |
| Implementation          | 5 days   | 2025-07-15 | 2025-07-21 |
| Testing & Fixes         | 3 days   | 2025-07-22 | 2025-07-24 |
| Release & Packaging     | 1 day    | 2025-07-25 | 2025-07-25 |

## 11. Risks & Mitigations
- **R1:** Registry permissions → request user-level only
- **R2:** Missing Git/npm → detect and prompt install
- **R3:** Long-running operations → use async + spinner

## 12. Technical Implementation Details

### 12.1 Registry Operations
- **Target:** `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings`
- **Keys:** `ProxyEnable`, `ProxyServer`, `ProxyOverride`
- **Method:** Use `Microsoft.Win32.Registry` namespace

### 12.2 Git Configuration
- **Commands:** 
  - Enable: `git config --global http.proxy [proxy_url]`
  - Disable: `git config --global --unset http.proxy`
- **Method:** Process execution with `System.Diagnostics.Process`

### 12.3 npm Configuration
- **Commands:**
  - Enable: `npm config set proxy [proxy_url]`
  - Disable: `npm config delete proxy`
- **Method:** Process execution with `System.Diagnostics.Process`

### 12.4 Environment Variables
- **Variables:** `HTTP_PROXY`, `HTTPS_PROXY`, `NO_PROXY`
- **Method:** `System.Environment.SetEnvironmentVariable`

## 13. Security Considerations
- **Principle of Least Privilege:** Only modify user-level settings
- **Input Validation:** Validate proxy URLs and settings
- **Error Logging:** Avoid logging sensitive proxy credentials
- **UAC Handling:** Design to work without elevated privileges

## 14. Performance Considerations
- **Async Operations:** All network and registry operations async
- **UI Threading:** Use `Dispatcher.Invoke` for UI updates
- **Caching:** Cache current proxy status to avoid repeated registry reads
- **Startup Optimization:** Load only essential components on startup

## 15. Deployment Strategy
- **Single-file EXE:** Use .NET 7 single-file publishing
- **Dependencies:** Self-contained deployment (no .NET runtime required)
- **Size Optimization:** Trim unused assemblies
- **Distribution:** Direct download or installer package

---

**Document Status:** Draft  
**Approved By:** [Pending]
