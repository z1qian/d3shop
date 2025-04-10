﻿using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record SetAdminUserSpecificPermissions(AdminUserId Id, IEnumerable<string> PermissionCodes)
    : ICommand;

public class SetAdminUserSpecificPermissionsCommandHandler(IAdminUserRepository adminUserRepository)
    : ICommandHandler<SetAdminUserSpecificPermissions>
{
    public async Task Handle(SetAdminUserSpecificPermissions request, CancellationToken cancellationToken)
    {
        var adminUser = await adminUserRepository.GetAsync(request.Id, cancellationToken) ??
                        throw new KnownException($"用户不存在，AdminUserId={request.Id}");

        var permissions = request.PermissionCodes.Select(code => new AdminUserPermission(code));

        adminUser.SetSpecificPermissions(permissions);
    }
}