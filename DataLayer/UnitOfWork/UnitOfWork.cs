using DataLayer.Data;
using DataLayer.Interface;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext appDbContext;
    private IDbContextTransaction transaction;

    public INotificationRepository NotificationRepository => new NotificationRepository(appDbContext);

    public IEmployeeRepository EmployeeRepository => new EmployeeRepository(appDbContext);
    public IAllocationRepository AllocationRepository => new AllocationRepository(appDbContext);
    public ILookUpRepository LookUpRepository => new LookUpRepository(appDbContext);
    public IProjectRepository ProjectRepository => new ProjectRepository(appDbContext);

    public IProjectRepository projectRepository => new ProjectRepository(appDbContext);

    public UnitOfWork(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }
    public async Task<bool> CompleteAsync()
    {
        return await appDbContext.SaveChangesAsync() > 0;
    }

    public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
    {
        return new GenericRepository<TEntity>(appDbContext);
    }


    public void BeginTransaction()
    {
        transaction = appDbContext.Database.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            transaction.Dispose();
        }
    }

    public void Rollback()
    {
        transaction.Rollback();
        transaction.Dispose();
    }
}
public interface IUnitOfWork
{
    INotificationRepository NotificationRepository { get; }

    IEmployeeRepository EmployeeRepository { get; }
    ILookUpRepository LookUpRepository { get; }
    IAllocationRepository AllocationRepository { get; }
    public IProjectRepository ProjectRepository { get; }
    IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;
    Task<bool> CompleteAsync();
    void BeginTransaction();
    void Commit();
    void Rollback();

}


