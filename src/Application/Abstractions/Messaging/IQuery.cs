using Solution.Application.Common;
using MediatR;

namespace Solution.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
