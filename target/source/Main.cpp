
#include <libmint>
#include <json.hpp>
#include <fstream>
#include <sys/stat.h>

using JSON = nlohmann::json;

void Cleanup()
{
    romfsExit();
    js::Finalize();
}

void ThrowCritical(std::string err)
{
    consoleExit(NULL);
    consoleInit(NULL);
    printf("\nMint runtime\n  -----------------------------------------------------------  \nA critical error ocurred:\n" CONSOLE_RED "%s" CONSOLE_RESET "\n\nPress any key to exit.", err.c_str());
    consoleUpdate(NULL);
    while(true)
    {
        hidScanInput();
        if(hidKeysDown(CONTROLLER_P1_AUTO)) break;
    }
    consoleExit(NULL);
    Cleanup();
    exit(0);
}

int main()
{
    js::Initialize();

    // Initialize QuickJS's ('std' and 'os') modules
    js::PushQuickJSModules();
    
    // Push custom modules
    libmint::InitializeModules();

    auto rc = romfsInit();
    if(R_FAILED(rc)) ThrowCritical("Unable to init RomFs.");

    std::ifstream ifs("romfs:/.mint/meta.json");
    if(!ifs.good()) ThrowCritical("Unable to open project's metadata JSON file.");
    auto json = JSON::parse(ifs);
    if(!json.count("script_table")) ThrowCritical("Unable to find the script file list of the application.");

    for(auto &script: json["script_table"])
    {
        auto fscript = script.get<std::string>();
        struct stat st;
        if(stat(("romfs:/.mint/src/" + fscript).c_str(), &st) != 0) ThrowCritical("A listed script file was not found: '" + fscript + "'");

        js::EvaluateFromFile("romfs:/.mint/src/" + fscript);
    }

    // After executing the project, proper cleanup
    Cleanup();
    return 0;
}