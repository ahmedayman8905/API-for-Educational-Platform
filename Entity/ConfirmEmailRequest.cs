using System.ComponentModel.DataAnnotations;

namespace Api_1.Entity;

public class ConfirmEmailRequest
{
    public string code { get; set; }
    
    public string id { get; set; }
}
