using Solution.Application.Common;
using MediatR;

namespace Solution.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IUnitOfWorkRequest;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IUnitOfWorkRequest;
