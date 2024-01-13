namespace ftreel.Dto.error;

public class SetMailTemplateDTO
{
    public string CustomSubject { get; set; }
    public string CustomBody { get; set; }

    public SetMailTemplateDTO(string customSubject, string customBody)
    {
        CustomSubject = customSubject;
        CustomBody = customBody;
    }
}