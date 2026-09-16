TFM      := netstandard2.1
CONFIG   := Debug
DLL      := VGTractorAuto.dll

BUILDDIR := VGTractorAuto/bin/$(CONFIG)/$(TFM)
BUILDDLL := $(BUILDDIR)/$(DLL)

# WSL path to the game install — adjust if Steam lives elsewhere
GAME_DIR := /mnt/c/Program Files (x86)/Steam/steamapps/common/Vanguard Galaxy
PLUGIN_DIR := $(GAME_DIR)/BepInEx/plugins/VGTractorAuto

VGAPI_DLL ?= ../vanguard-galaxy-api/VGModAPI.Abstractions/bin/Debug/netstandard2.1/VGModAPI.Abstractions.dll

# Resolve dotnet — prefer explicit local SDK, fall back to PATH
DOTNET   ?= $(shell command -v dotnet 2>/dev/null || echo /tmp/dnsdk/dotnet/dotnet)

.PHONY: all build test clean deploy check-bepinex

all: build

check-bepinex:
	@test -d "$(GAME_DIR)/BepInEx/plugins" || { \
		echo "BepInEx plugins dir not found at $(GAME_DIR)/BepInEx/plugins." ; \
		echo "Install BepInEx 5.x into the game folder and launch the game once." ; \
		exit 1 ; \
	}

build:
	@test -s "$(VGAPI_DLL)" || { echo 'Set VGAPI_DLL to the tractor-beam API build.'; exit 1; }
	DOTNET_ROOT=$(dir $(DOTNET)) $(DOTNET) build VGTractorAuto/VGTractorAuto.csproj -c $(CONFIG) -p:VGAPI_DLL="$(abspath $(VGAPI_DLL))"

test:
	$(DOTNET) test VGTractorAuto.Tests/VGTractorAuto.Tests.csproj -c $(CONFIG)

deploy: build check-bepinex
	@mkdir -p "$(PLUGIN_DIR)"
	cp "$(BUILDDLL)" "$(PLUGIN_DIR)/"
	@if [ -f "$(BUILDDIR)/VGTractorAuto.pdb" ]; then cp "$(BUILDDIR)/VGTractorAuto.pdb" "$(PLUGIN_DIR)/"; fi
	@echo "Deployed $(DLL) to $(PLUGIN_DIR)"

clean:
	$(DOTNET) clean VGTractorAuto/VGTractorAuto.csproj
	rm -rf VGTractorAuto/bin VGTractorAuto/obj
