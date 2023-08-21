using Globomantics.Domain;
using Microsoft.EntityFrameworkCore;

namespace Globomantics.Infrastructure.Data.Repositories;

public class FeatureRepository : ToDoRepository<Feature>
{
    public FeatureRepository(GlobomanticsDbContext context) : base(context)
    {
    }
    public override async Task AddAsync(Domain.Feature feature)
    {
        var existingFeature = await Context.Features.FirstOrDefaultAsync(b => b.Id == feature.Id);
        var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == feature.CreatedBy.Id);

        user ??= new() { Id = feature.CreatedBy.Id, Name = feature.CreatedBy.Name };

        if (existingFeature is not null)
        {
            await UpdateAsync(feature, existingFeature, user);
        }
        else
        {
            await CreateAsync(feature, user);
        }
    }

    private async Task CreateAsync(Feature feature, Models.User user)
    {
        var featureToAdd = DomainToDataMapping.MapToDoFromDomain<Feature, Data.Models.Feature>(feature);

        await SetParentAsync(featureToAdd, feature);

        featureToAdd.CreatedBy = user;
        featureToAdd.AssignedTo = user;


        await Context.AddAsync(featureToAdd);
    }

    private async Task UpdateAsync(Feature feature, Data.Models.Feature featureToUpdate, Data.Models.User user)
    {
        await SetParentAsync(featureToUpdate, feature);

        featureToUpdate.IsCompleted = feature.IsCompleted;
        featureToUpdate.Component = feature.Component;
        featureToUpdate.Priority = feature.Priority;
        featureToUpdate.AssignedTo = user;
        featureToUpdate.CreatedBy = user;
        featureToUpdate.Title = feature.Title;
        featureToUpdate.Description = feature.Description;

        Context.Features.Update(featureToUpdate);
    }

    public override async Task<Domain.Feature> GetAsync(Guid id)
    {
        var data = await Context.Features.SingleAsync(feature => feature.Id == id);

        return DataToDomainMapping.MapToDoFromData<Data.Models.Feature, Domain.Feature>(data);
    }
}
