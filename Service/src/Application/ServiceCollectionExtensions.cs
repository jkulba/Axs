using Application.AccessRequests.Commands;
using Application.AccessRequests.Queries;
using Application.Activities.Commands;
using Application.Activities.Queries;
using Application.Authorization.Commands;
using Application.Behaviors;
using Application.Common;
using Application.Interfaces;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Common;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register your dispatchers
        services.AddTransient<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<IQueryDispatcher, QueryDispatcher>();

        // Register all default pipeline behaviors
        services.AddScoped(typeof(IQueryPipelineBehavior<,>), typeof(QueryValidationBehavior<,>));
        services.AddScoped(typeof(IQueryPipelineBehavior<,>), typeof(QueryPerformanceBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandPerformanceBehavior<,>));

        // services.AddQueryPipelineBehaviors();
        // services.AddCommandPipelineBehaviors();

        // Register FluentValidation (includes validators and validation behaviors)
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        // Register Query Handlers here
        services.AddScoped<IQueryHandler<GetAccessRequestsQuery, Result<IEnumerable<AccessRequest>>>, GetAccessRequestsQueryHandler>();
        services.AddScoped<IQueryHandler<GetAccessRequestsByJobNumberQuery, Result<IEnumerable<AccessRequest>>>, GetAccessRequestsByJobNumberQueryHandler>();
        services.AddScoped<IQueryHandler<GetAccessRequestsByUserNameQuery, Result<IEnumerable<AccessRequest>>>, GetAccessRequestsByUserNameQueryHandler>();
        services.AddScoped<IQueryHandler<GetAccessRequestByIdQuery, Result<AccessRequest>>, GetAccessRequestByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetAccessRequestByRequestCodeQuery, Result<AccessRequest>>, GetAccessRequestByRequestCodeQueryHandler>();
        services.AddScoped<IQueryHandler<ExistsAccessRequestByRequestCodeQuery, Result<bool>>, ExistsAccessRequestByRequestCodeQueryHandler>();
        services.AddScoped<IQueryHandler<SearchAccessRequestsQuery, Result<IEnumerable<AccessRequest>>>, SearchAccessRequestsQueryHandler>();

        services.AddScoped<IQueryHandler<GetActivityByIdQuery, Result<Activity>>, GetActivityByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetActivitiesQuery, Result<IEnumerable<Activity>>>, GetActivitiesQueryHandler>();

        services.AddScoped<IQueryHandler<GetUserByIdQuery, Result<User>>, GetUserByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetUserByUserIdQuery, Result<User>>, GetUserByUserIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetUsersQuery, Result<IEnumerable<User>>>, GetUsersQueryHandler>();

        // Register Command Handlers here
        services.AddScoped<ICommandHandler<CreateAccessRequestCommand, Result<AccessRequest>>, CreateAccessRequestCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteAccessRequestCommand, Result<int>>, DeleteAccessRequestCommandHandler>();
        services.AddScoped<ICommandHandler<VerifyAccessCommand, Result<AuthorizationResult>>, VerifyAccessCommandHandler>();

        services.AddScoped<ICommandHandler<CreateActivityCommand, Result<Activity>>, CreateActivityCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateActivityCommand, Result<Activity>>, UpdateActivityCommandHandler>();

        services.AddScoped<ICommandHandler<CreateUserCommand, Result<User>>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand, Result<User>>, UpdateUserCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand, Result<int>>, DeleteUserCommandHandler>();


        return services;
    }
}