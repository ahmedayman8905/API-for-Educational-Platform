using Api_1.Contract;
using Mapster;

namespace Api_1.mappstar;

public class MappingConfig : IRegister

{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<Student, StudentResponse>()
        //    .Map(dest => dest.id, src => src.Id);


    }
}
