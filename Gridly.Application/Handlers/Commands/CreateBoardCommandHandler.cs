using ErrorOr;

using Loot.Application.Commands;
using Loot.Application.Dtos;
using Loot.Domain.Entities;
using Loot.Domain.Enums;
using Loot.Infrastructure.DbContext;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Loot.Application.Handlers.Commands;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, ErrorOr<BoardDto>>
{
    private readonly LootDbContext _lootDbContext;
    private readonly UserManager<AppUser> _userManager; 
        
    public CreateBoardCommandHandler(LootDbContext lootDbContext, UserManager<AppUser> userManager)
    {
        _lootDbContext = lootDbContext;
        _userManager = userManager;
    }

    public async Task<ErrorOr<BoardDto>> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return Error.NotFound("User.NotFound", "User not found");
        
        var board = new Board() { Name = request.Name, CreatedAt = DateTimeOffset.UtcNow};
        var member = new BoardMember() { UserId = user.Id, AppUser = user, BoardId = board.Id, Board = board, Role = BoardRole.Owner };
        board.Members.Add(member);
        
        var createdBoard = await _lootDbContext.Boards.AddAsync(board, cancellationToken);
        await _lootDbContext.SaveChangesAsync(cancellationToken);
        
        return new BoardDto() { Id = createdBoard.Entity.Id, Name = createdBoard.Entity.Name, };
    }
}