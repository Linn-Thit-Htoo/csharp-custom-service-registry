namespace CustomServiceRegistry.RegistryApi.Features.Tenant.Core;

public class TenantService : ITenantService
{
    private readonly IMongoCollection<TenantCollection> _tenantCollection;

    public TenantService()
    {
        _tenantCollection = CollectionNames.TenantCollection.GetCollection<TenantCollection>();
    }

    public async Task<Result<CreateTenantResponse>> CreateTenantAsync(
        CreateTenantCommand command,
        CancellationToken cs = default
    )
    {
        var tenant = command.ToCollection();
        await _tenantCollection.InsertOneAsync(tenant, cancellationToken: cs);

        return Result<CreateTenantResponse>.Success(
            new CreateTenantResponse() { ApiKey = tenant.TenantId.ToString() }
        );
    }

    public async Task<Result<GetTenantByIdResponse>> GetTenantByIdAsync(
        string tenantId,
        CancellationToken cancellationToken = default
    )
    {
        Result<GetTenantByIdResponse> result;

        var item = await _tenantCollection
            .Find(x => x.TenantId == Guid.Parse(tenantId))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (item is null)
        {
            result = Result<GetTenantByIdResponse>.Fail("No data found.");
            goto result;
        }

        result = Result<GetTenantByIdResponse>.Success(
            new GetTenantByIdResponse()
            {
                TenantId = item.TenantId,
                ApplicationName = item.ApplicationName,
            }
        );

    result:
        return result;
    }
}
