# VS Code Configuration for File-Scoped Namespaces

## Global Settings (Manual Setup Required)

To configure VS Code globally for file-scoped namespaces, you'll need to manually edit your global VS Code settings:

### 1. Open VS Code Settings
- Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
- Type "Preferences: Open Settings (JSON)"
- Add these settings to your global settings.json:

```json
{
    "omnisharp.enableEditorConfigSupport": true,
    "omnisharp.enableRoslynAnalyzers": true,
    "csharp.format.enable": true,
    "editor.formatOnSave": true,
    "editor.formatOnType": true,
    "editor.codeActionsOnSave": {
        "source.organizeImports": "explicit",
        "source.fixAll": "explicit"
    },
    "dotnet.preferCSharpExtension": true
}
```

### 2. Create Global .editorconfig (Optional)
Create a file at `~/.editorconfig` with:

```ini
root = true

[*.cs]
csharp_style_namespace_declarations = file_scoped:suggestion
```

## Project-Specific Configuration (Already Set Up)

The following files have been configured for this workspace:
- `.editorconfig` - Main configuration for file-scoped namespaces
- `.vscode/settings.json` - Workspace-specific VS Code settings
- `omnisharp.json` - OmniSharp configuration
- `.vscode/launch.json` - Debug configuration
- `.vscode/tasks.json` - Build tasks

## How to Test

1. Create a new C# class file
2. The namespace should automatically be generated with file-scoped syntax: `namespace YourNamespace;`
3. When you format the document (Ctrl+K, Ctrl+D), existing block namespaces should be converted to file-scoped

## Key Setting

The most important setting is in `.editorconfig`:
```ini
csharp_style_namespace_declarations = file_scoped:suggestion
```

This tells the C# formatter to prefer file-scoped namespaces when creating new files or refactoring existing ones.
