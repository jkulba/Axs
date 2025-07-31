using Ardalis.SmartEnum;

namespace Domain.Common;

/// <summary>
/// Represents the different types of objects in the system using a type-safe enumeration pattern.
/// This class extends SmartEnum to provide strongly-typed object type definitions with associated
/// display names and unique integer values for various document and content types.
/// </summary>
/// <remarks>
/// ObjectType includes various RRD (Render Repository Document) types, PDMS (Profile Document Management System) types,
/// and other system-specific object classifications. Each object type has a unique identifier and display name
/// that can be used for database operations, API responses, and user interfaces.
/// </remarks>
public class ObjectType : SmartEnum<ObjectType>
{
    public static readonly ObjectType All = new ObjectType(nameof(All), "ALL", 0);
    public static readonly ObjectType RrdAssembly = new ObjectType(nameof(RrdAssembly), "rrd_assembly", 1);
    public static readonly ObjectType RrdBrowser = new ObjectType(nameof(RrdBrowser), "rrd_browser", 2);
    public static readonly ObjectType RrdControl = new ObjectType(nameof(RrdControl), "rrd_control", 3);
    public static readonly ObjectType RrdCosmeticEdit = new ObjectType(nameof(RrdCosmeticEdit), "rrd_cosmetic_edit", 4);
    public static readonly ObjectType RrdDoclet = new ObjectType(nameof(RrdDoclet), "rrd_doclet", 5);
    public static readonly ObjectType RrdDocument = new ObjectType(nameof(RrdDocument), "rrd_document", 6);
    public static readonly ObjectType RrdEdgarSubmission = new ObjectType(nameof(RrdEdgarSubmission), "rrd_edgar_submission", 7);
    public static readonly ObjectType RrdGraphic = new ObjectType(nameof(RrdGraphic), "rrd_graphic", 9);
    public static readonly ObjectType RrdReport = new ObjectType(nameof(RrdReport), "rrd_report", 10);
    public static readonly ObjectType RrdSource = new ObjectType(nameof(RrdSource), "rrd_source", 11);
    public static readonly ObjectType RrdStyle = new ObjectType(nameof(RrdStyle), "rrd_style", 12);
    public static readonly ObjectType PdmsBaseDoc = new ObjectType(nameof(PdmsBaseDoc), "pdms_base_doc", 176);
    public static readonly ObjectType PdmsRendition = new ObjectType(nameof(PdmsRendition), "pdms_rendition", 192);
    public static readonly ObjectType RrdVdm = new ObjectType(nameof(RrdVdm), "rrd_vdm", 208);
    public static readonly ObjectType RrdCabinet = new ObjectType(nameof(RrdCabinet), "rrd_cabinet", 224);
    public static readonly ObjectType RrdFolder = new ObjectType(nameof(RrdFolder), "rrd_folder", 240);
    public static readonly ObjectType RrdJobFolder = new ObjectType(nameof(RrdJobFolder), "rrd_job_folder", 241);

    public string DisplayName { get; }
    public static ObjectType FromDisplayName(string displayName)
    {
        return List.FirstOrDefault(dt => dt.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase))
               ?? throw new ArgumentException($"No ObjectType with display name '{displayName}' found.", nameof(displayName));
    }

    protected ObjectType(string name, string displayName, int value) : base(name, value)
    {
        DisplayName = displayName;
    }
}
