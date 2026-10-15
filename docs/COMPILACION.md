# ?? Compilación y Construcción

Guía completa para compilar Media Downloader desde el código fuente.

---

## Requisitos Previos

### Mínimos
- **Windows**: 7 o superior
- **.NET 8.0 SDK**: Descárga desde https://dotnet.microsoft.com/download/dotnet/8.0
- **Visual Studio 2022** (opcional pero recomendado)

### Verificar instalación
```bash
# Verificar que .NET está instalado
dotnet --version

# Debería mostrar algo como: 8.0.x
```

---

## Método 1: Visual Studio 2022 (Recomendado)

### Paso 1: Abrir la solución
1. Abre Visual Studio 2022
2. Archivo > Abrir > Proyecto/Solución
3. Navega a la carpeta `MediaDownloader`
4. Selecciona `MediaDownloader.csproj`

### Paso 2: Compilar
**Opción A: Menú**
1. Compilar > Compilar solución
2. Atajo: `Ctrl + Shift + B`

**Opción B: Explorador de soluciones**
1. Haz clic derecho en el proyecto
2. Compilar

### Paso 3: Ejecutar
1. Presiona `F5` (depuración)
2. O `Ctrl + F5` (sin depuración)

### Resultado
- ? La aplicación se ejecutará
- ? Cualquier error se mostrará en la ventana de salida
- ? Encontrarás el ejecutable en `bin/Debug/net8.0-windows/`

---

## Método 2: Línea de Comandos (PowerShell/CMD)

### Paso 1: Navegar a la carpeta
```bash
cd D:\VS#\MediaDownloader\
```

### Paso 2: Restaurar dependencias
```bash
dotnet restore
```

### Paso 3: Compilar
```bash
# Debug (con símbolos de depuración)
dotnet build

# O Release (optimizado)
dotnet build --configuration Release
```

### Paso 4: Ejecutar
```bash
# Desde la carpeta del proyecto
dotnet run

# O directamente el ejecutable
.\bin\Debug\net8.0-windows\MediaDownloader.exe
```

### Resultado
```
Build succeeded.
Media Downloader iniciará automáticamente
```

---

## Método 3: Compilación Avanzada

### Compilar sin limpiar
```bash
dotnet build --no-restore
```

### Compilar modo Release (más rápido)
```bash
dotnet build --configuration Release
dotnet run --configuration Release
```

### Publicar como ejecutable standalone
```bash
dotnet publish -c Release -o ./publish
```

### Limpiar archivos compilados
```bash
dotnet clean
```

---

## Estructura de Carpetas Después de Compilar

```
MediaDownloader/
??? bin/
?   ??? Debug/
?   ?   ??? net8.0-windows/
?   ?       ??? MediaDownloader.exe     ? Ejecutable (Debug)
?   ?       ??? MediaDownloader.dll
?   ?       ??? [dependencias]
?   ??? Release/
?       ??? net8.0-windows/
?           ??? MediaDownloader.exe     ? Ejecutable (Release)
?           ??? MediaDownloader.dll
?           ??? [dependencias]
??? obj/
?   ??? Debug/
?   ??? Release/
??? [otros archivos]
```

---

## Solución de Problemas de Compilación

### Error: "The project file could not be loaded"
**Causa**: Archivo .csproj corrupto
**Solución**:
```bash
dotnet clean
dotnet restore
dotnet build
```

### Error: "No se encontró '.NET 8.0'"
**Causa**: .NET 8.0 SDK no está instalado
**Solución**:
1. Descarga .NET 8.0 desde https://dotnet.microsoft.com/download/dotnet/8.0
2. Instala el SDK
3. Reinicia Visual Studio o terminal

### Error: "Proyecto ya está cargado"
**Causa**: Instancia anterior de la aplicación aún ejecutándose
**Solución**:
```bash
# En Administrador de Tareas (Ctrl + Shift + Esc)
# Busca "MediaDownloader" y hazle "Finalizar tarea"
```

### Error: "Acceso denegado"
**Causa**: Permisos insuficientes
**Solución**:
- Ejecuta Visual Studio como administrador
- O ejecuta PowerShell como administrador:
```bash
Run as Administrator
```

---

## Verificar Compilación Exitosa

### Checklist de verificación
- [ ] Compilación sin errores
- [ ] Compilación sin advertencias críticas
- [ ] Ejecutable se crea en `bin/Debug` o `bin/Release`
- [ ] La aplicación inicia correctamente
- [ ] La interfaz se ve bien
- [ ] Los botones responden

### Validar que funciona
1. Compila la aplicación
2. Ejecuta `MediaDownloader.exe`
3. Intenta pegar una URL de YouTube o X
4. Haz clic en "Descargar"
5. Verifica que empiece el proceso

---

## Optimizaciones de Compilación

### Para desarrollo rápido
```bash
dotnet build  # Es rápido, solo compila cambios
dotnet run
```

### Para producción
```bash
dotnet publish -c Release --self-contained -r win-x64
```

Esto crea un ejecutable que incluye .NET, muy útil para distribución.

### Compilar sin generar archivos temporales
```bash
dotnet build --no-incremental
```

---

## Configuración del Proyecto (csproj)

El archivo `MediaDownloader.csproj` contiene:

```xml
<PropertyGroup>
  <OutputType>WinExe</OutputType>           <!-- Ejecutable Windows -->
  <TargetFramework>net8.0-windows</TargetFramework>  <!-- .NET 8 -->
  <Nullable>enable</Nullable>               <!-- Verificación nula estricta -->
  <ImplicitUsings>enable</ImplicitUsings>   <!-- Usar implícitos globales -->
  <UseWPF>true</UseWPF>                     <!-- Activar WPF -->
</PropertyGroup>
```

Para modificar estos valores, edita el archivo `.csproj`.

---

## Integración Continua (CI/CD)

### Para GitHub Actions
Crea `.github/workflows/build.yml`:

```yaml
name: .NET Build

on: [push, pull_request]

jobs:
  build:
    runs-on: windows-latest
    steps:
    - uses: actions/checkout@v2
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '8.0.x'
    - name: Restore
      run: dotnet restore
    - name: Build
      run: dotnet build --configuration Release
    - name: Upload artifact
      uses: actions/upload-artifact@v2
      with:
        name: MediaDownloader
        path: bin/Release/net8.0-windows/
```

---

## Distribución del Ejecutable

### Opción 1: Archivo EXE simple
```bash
dotnet publish -c Release -o publish
# Encuentra el .exe en publish/
```

### Opción 2: Ejecutable autocontenido (no requiere .NET)
```bash
dotnet publish -c Release --self-contained -r win-x64 -o publish
```

### Opción 3: Crear instalador (MSI)
Requiere WiX Toolset (avanzado)

---

## Comandos Útiles Rápidos

```bash
# Compilar y ejecutar en una sola línea
dotnet run

# Compilar silenciosamente
dotnet build --verbosity quiet

# Mostrar información detallada
dotnet build --verbosity diagnostic

# Compilar solo cambios
dotnet build --incremental

# Forzar compilación completa
dotnet build --no-incremental

# Ver las propiedades del proyecto
dotnet project-info

# Restaurar solo dependencias NuGet
dotnet restore
```

---

## Depuración

### En Visual Studio
1. Presiona `F5` para iniciar con depuración
2. Los breakpoints funcionarán automáticamente
3. La salida aparecerá en la ventana de depuración

### En Línea de Comandos
```bash
# Ejecutar con depuración
dotnet run --configuration Debug

# Ver más detalles
dotnet run --configuration Debug --verbose
```

---

## Rendimiento de Compilación

### Compilaciones típicas
- **Inicial**: 30-60 segundos (primera vez)
- **Subsecuentes**: 2-5 segundos (cambios menores)
- **Limpiar + Compilar**: 30-60 segundos

### Acelerar compilación
1. Usa `Release` en lugar de `Debug` para producción
2. Evita cambios en archivos XAML si es posible
3. Usa incremental building (por defecto)

---

## Resumen

| Tarea | Comando |
|-------|---------|
| Compilar | `dotnet build` |
| Ejecutar | `dotnet run` |
| Limpiar | `dotnet clean` |
| Restaurar | `dotnet restore` |
| Release | `dotnet build -c Release` |
| Publicar | `dotnet publish -c Release` |

---

**¡Compilación exitosa!**

Si tienes problemas, consulta [FAQ.md](FAQ.md) o [README.md](README.md).
