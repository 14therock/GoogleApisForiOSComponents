using ObjCRuntime;

[assembly: LinkWith("GoogleMaps",
    LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.Arm64,
    Frameworks = "Accelerate GLKit ImageIO OpenGLES CoreText Security",
    LinkerFlags = "-ObjC",
    SmartLink = true,
    ForceLoad = true)]

[assembly: LinkWith("GoogleMapsCore",
    LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.Arm64,
    Frameworks = "CoreTelephony",
    SmartLink = true,
    ForceLoad = true)]

[assembly: LinkWith("GoogleMapsBase",
    LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.Arm64,
    Frameworks = "CoreData SystemConfiguration QuartzCore CoreGraphics CoreLocation UIKit",
    LinkerFlags = "-ObjC -lz -lc++",
    SmartLink = true,
    ForceLoad = true)]
