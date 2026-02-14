#!/bin/sh
# install.sh - Unified installer for OpenClawWallet (Server + CLI)

set -e

REPO="appfi5/superise-for-agent"
INSTALL_DIR="$HOME/superise-for-agent"
LOG_FILE="$INSTALL_DIR/server.log"

# Detect OS and architecture
UNAME_S=$(uname -s)
UNAME_M=$(uname -m)

OS=""
ARCH=""

case "$UNAME_S" in
    Linux*)
        OS="linux"
        ;;
    Darwin*)
        OS="osx"
        ;;
    *)
        echo "❌ Unsupported OS: $UNAME_S"
        exit 1
        ;;
esac

case "$UNAME_M" in
    x86_64|amd64)
        ARCH="x64"
        ;;
    aarch64|arm64)
        if [ "$OS" = "osx" ]; then
            ARCH="arm64"
        else
            ARCH="x64"
        fi
        ;;
    *)
        echo "❌ Unsupported architecture: $UNAME_M"
        exit 1
        ;;
esac

# Calculate RID for server
if [ "$OS" = "osx" ] && [ "$UNAME_M" = "arm64" ]; then
    ARCH="arm64"
fi

if [ "$OS" = "osx" ]; then
    if [ "$ARCH" = "arm64" ]; then
        RID="osx-arm64"
    else
        RID="osx-x64"
    fi
else
    if [ "$ARCH" = "arm64" ]; then
        RID="linux-arm64"
    else
        RID="linux-x64"
    fi
fi

echo "🔍 Detected OS: $OS, Arch: $UNAME_M → RID: ${RID}"
echo "📁 Install directory: $INSTALL_DIR"
echo ""

# Function to install server
install_server() {
    echo "📦 Installing SupeRISELocal Server..."
    echo ""

    APP_NAME="SupeRISELocalServer"
    FILE="${APP_NAME}-${RID}.tar.gz"
    URL="https://github.com/${REPO}/releases/latest/download/${FILE}"

    # Create install dir if not exists
    if [ ! -d "$INSTALL_DIR" ]; then
        mkdir -p "$INSTALL_DIR"
    fi

    cd "$INSTALL_DIR"

    # Check if files exist
    EXECUTABLE_EXISTS=false
    CONFIG_EXISTS=false

    if [ -f "$APP_NAME" ]; then
        EXECUTABLE_EXISTS=true
    fi

    if [ -f "appsettings.json" ] || [ -f "appsettings.Production.json" ]; then
        CONFIG_EXISTS=true
    fi

    DOWNLOAD=false

    # Ask user if files exist
    if [ "$EXECUTABLE_EXISTS" = true ] && [ "$CONFIG_EXISTS" = true ]; then
        echo "📦 Found existing server installation in $INSTALL_DIR"
        if [ -t 0 ]; then
            printf "Do you want to download the latest version? [y/N]: "
            read -r response
            if [ "$response" != "y" ] && [ "$response" != "Y" ]; then
                echo "🚀 Starting existing version..."
            else
                DOWNLOAD=true
                echo "⬇️ Downloading latest version..."
            fi
        else
            DOWNLOAD=true
            echo "⬇️ Downloading latest version (default in non-interactive mode)"
        fi
    else
        DOWNLOAD=true
        echo "📦 Server installation not found, downloading latest version..."
    fi

    # Download and extract if needed
    if [ "$DOWNLOAD" = true ]; then
        BACKUP_DIR="$INSTALL_DIR/.backup"
        BACKUP_CREATED=false

        if [ "$EXECUTABLE_EXISTS" = true ] || [ "$CONFIG_EXISTS" = true ]; then
            echo "📦 Backing up existing files..."
            mkdir -p "$BACKUP_DIR"
            if [ -f "$APP_NAME" ]; then
                mv "$APP_NAME" "$BACKUP_DIR/"
                BACKUP_CREATED=true
            fi
            if [ -f "appsettings.json" ]; then
                mv "appsettings.json" "$BACKUP_DIR/"
                BACKUP_CREATED=true
            fi
            if [ -f "appsettings.Production.json" ]; then
                mv "appsettings.Production.json" "$BACKUP_DIR/"
                BACKUP_CREATED=true
            fi
        fi

        echo "⬇️ Downloading $FILE..."
        if ! curl -L --progress-bar "$URL" | tar -xz; then
            echo "❌ Download or extraction failed!"

            if [ "$BACKUP_CREATED" = true ]; then
                echo "🔄 Restoring backup..."
                mv "$BACKUP_DIR"/* .
                rm -rf "$BACKUP_DIR"
            fi

            echo "Please check your network connection and try again."
            exit 1
        fi

        if [ ! -f "$APP_NAME" ]; then
            echo "❌ Executable file not found after extraction!"

            if [ "$BACKUP_CREATED" = true ]; then
                echo "🔄 Restoring backup..."
                mv "$BACKUP_DIR"/* .
                rm -rf "$BACKUP_DIR"
            fi

            exit 1
        fi

        if [ ! -f "appsettings.json" ] && [ ! -f "appsettings.Production.json" ]; then
            echo "❌ Configuration file not found after extraction!"

            if [ "$BACKUP_CREATED" = true ]; then
                echo "🔄 Restoring backup..."
                mv "$BACKUP_DIR"/* .
                rm -rf "$BACKUP_DIR"
            fi

            exit 1
        fi

        chmod +x "$APP_NAME"

        if [ "$BACKUP_CREATED" = true ]; then
            rm -rf "$BACKUP_DIR"
            echo "🗑️ Backup removed"
        fi

        echo "✅ Download and extraction completed"
    fi

    # Create log file if not exists
    if [ ! -f "$LOG_FILE" ]; then
        touch "$LOG_FILE"
    fi

    # Stop old process
    pkill -f "$APP_NAME" 2>/dev/null || true

    # Start in background
    nohup "./$APP_NAME" --urls "http://*:18799" > "$LOG_FILE" 2>&1 &
    sleep 2

    # Get IP for Linux
    if [ "$OS" = "Linux" ]; then
        IP=$(hostname -I | awk '{print $1}')
    else
        IP="localhost"
    fi

    echo ""
    echo "✅ SupeRISELocal Server started!"
    echo "🌐 Access at: http://$IP:18799/superise-for-agent/swagger/index.html"
    echo "📄 Log file: $LOG_FILE"
    echo "📁 Data directory: $INSTALL_DIR/data"
}

# Function to install CLI
install_cli() {
    echo "📦 Installing SupeRISE CLI..."
    echo ""

    APP_NAME="rise"
    FILE="${APP_NAME}-${OS}-${ARCH}.tar.gz"
    URL="https://github.com/${REPO}/releases/latest/download/${FILE}"

    # Create install dir if not exists
    if [ ! -d "$INSTALL_DIR" ]; then
        mkdir -p "$INSTALL_DIR"
    fi

    cd "$INSTALL_DIR"

    # Download and extract
    echo "⬇️ Downloading $FILE..."
    if ! curl -L --progress-bar "$URL" -o "$FILE"; then
        echo "❌ Download failed!"
        echo "Please check your network connection and try again."
        exit 1
    fi

    echo "📂 Extracting $FILE..."
    if ! tar -xzf "$FILE"; then
        echo "❌ Extraction failed!"
        rm -f "$FILE"
        exit 1
    fi

    rm -f "$FILE"

    # Make executable
    chmod +x "$APP_NAME"

    echo ""
    echo "✅ SupeRISE CLI installed successfully!"
    echo "📁 Executable: $INSTALL_DIR/$APP_NAME"

    # Detect current shell and add to PATH
    NEEDS_SOURCE=false
    SHELL_ADDED=""

    if [ -n "$SHELL" ]; then
        SHELL_NAME=$(basename "$SHELL")
        PROFILE=""

        case "$SHELL_NAME" in
            bash)
                PROFILE="$HOME/.bashrc"
                PATH_EXPORT="export PATH=\"\$PATH:$INSTALL_DIR\""
                ;;
            zsh)
                PROFILE="$HOME/.zshrc"
                PATH_EXPORT="export PATH=\"\$PATH:$INSTALL_DIR\""
                ;;
            fish)
                PROFILE="$HOME/.config/fish/config.fish"
                PATH_EXPORT="fish_add_path $INSTALL_DIR"
                ;;
        esac

        if [ -n "$PROFILE" ]; then
            # Create config directory for fish if not exists
            if [ "$SHELL_NAME" = "fish" ] && [ ! -d "$(dirname "$PROFILE")" ]; then
                mkdir -p "$(dirname "$PROFILE")"
            fi

            # Check if already in PATH
            if ! grep -q "$INSTALL_DIR" "$PROFILE" 2>/dev/null; then
                echo ""
                echo "🔧 Adding $INSTALL_DIR to PATH in $PROFILE"
                echo "$PATH_EXPORT" >> "$PROFILE"
                SHELL_ADDED="$PROFILE"
                NEEDS_SOURCE=true
            else
                echo "✓ $INSTALL_DIR is already in your PATH."
            fi
        fi
    fi

    echo ""
    echo "📌 To use the CLI immediately, run:"
    echo "   export PATH=\"\$PATH:$INSTALL_DIR\""
    echo ""
    if [ "$NEEDS_SOURCE" = true ] && [ -n "$SHELL_ADDED" ]; then
        echo "📌 Or restart your shell to apply PATH changes permanently."
    fi
    echo ""
    echo "🚀 Usage: $APP_NAME --help"
}

# Install both Server and CLI
INSTALL_SERVER=true
INSTALL_CLI=true

# Execute installations
if [ "$INSTALL_SERVER" = true ] && [ "$INSTALL_CLI" = true ]; then
    echo "🚀 Installing Server and CLI..."
    echo ""
    echo "=========================================="
    install_server
    echo ""
    echo "=========================================="
    echo ""
    install_cli
elif [ "$INSTALL_SERVER" = true ]; then
    install_server
elif [ "$INSTALL_CLI" = true ]; then
    install_cli
fi

echo ""
echo "✨ Installation complete!"
