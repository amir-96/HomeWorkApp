using Domain;
using Domain.BaseModels;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public class BaseService<TKey, T> : IRepository<TKey, T> where T : BaseModel
  {
    private readonly ApplicationDbContext _context;

    public BaseService(ApplicationDbContext context)
    {
      _context = context;
    }

    #region Get All

    public async Task<ServerResponse<List<T>>> Get()
    {
      try
      {
        var data = await _context.Set<T>()
          .ToListAsync();

        var response = new ServerResponse<List<T>>(true, "عملیات با موفقیت انجام شد", data);

        return response;
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<List<T>>(false, errorMessage, null);

        return response;
      }
    }

    #endregion

    #region Get By Id

    public async Task<ServerResponse<T>> Get(TKey id)
    {
      try
      {
        var data = await _context.FindAsync<T>(id);

        if (data == null)
        {
          var failureResponse = new ServerResponse<T>(false, "هیچ اطلاعاتی با این مشخصات پیدا نشد", null);

          return failureResponse;
        }

        var response = new ServerResponse<T>(true, "عملیات با موفقیت انجام شد", data);

        return response;
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<T>(false, errorMessage, null);

        return response;
      }
    }

    #endregion

    #region Create New

    public async Task<ServerResponse<bool>> Create(T entity)
    {
      try
      {
        var data = await _context.AddAsync<T>(entity);

        await _context.SaveChangesAsync();

        var response = new ServerResponse<bool>(true, "عملیات با موفقیت انجام شد", true);

        return response;
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<bool>(false, errorMessage, false);

        return response;
      }
    }

    #endregion

    #region Update

    public async Task<ServerResponse<bool>> Update(TKey id, T entityToUpdate)
    {
      try
      {
        var existingEntity = await _context.FindAsync<T>(id);

        if (existingEntity == null)
        {
          return new ServerResponse<bool>(false, "اطلاعات مورد نظر یافت نشد", false);
        }

        _context.Entry(existingEntity).CurrentValues.SetValues(entityToUpdate);

        await _context.SaveChangesAsync();

        return new ServerResponse<bool>(true, "اطلاعات با موفقیت به روز رسانی شد", true);
      }
      catch (Exception ex)
      {
        string errorMessage = "خطا در به روز رسانی اطلاعات : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<bool>(false, errorMessage, false);

        return response;
      }
    }


    #endregion

    #region Check Exists

    public async Task<ServerResponse<bool>> Exists(Expression<Func<T, bool>> expression)
    {
      try
      {
        var data = await _context.Set<T>().AnyAsync(expression);

        var response = new ServerResponse<bool>(true, "عملیات با موفقیت انجام شد", data);

        return response;
      }
      catch (Exception ex)
      {
        string errorMessage = "عملیات با خطا مواجه شد : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<bool>(false, errorMessage, false);

        return response;
      }
    }

    #endregion

    #region Delete

    public async Task<ServerResponse<bool>> Delete(TKey id)
    {
      try
      {
        var entity = await _context.FindAsync<T>(id);

        if (entity == null)
        {
          return new ServerResponse<bool>(false, "موجودیت مورد نظر یافت نشد", false);
        }

        entity.IsActive = false;

        await _context.SaveChangesAsync();

        return new ServerResponse<bool>(true, "موجودیت با موفقیت حذف شد.", true);
      }
      catch (Exception ex)
      {
        string errorMessage = "خطا در حذف موجودیت : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<bool>(false, errorMessage, false);

        return response;
      }
    }

    #endregion

    #region Restore

    public async Task<ServerResponse<bool>> Restore(TKey id)
    {
      try
      {
        var entity = await _context.FindAsync<T>(id);

        if (entity == null)
        {
          return new ServerResponse<bool>(false, "موجودیت مورد نظر یافت نشد", false);
        }

        entity.IsActive = true;

        await _context.SaveChangesAsync();

        return new ServerResponse<bool>(true, "موجودیت با موفقیت بازگردانی شد.", true);
      }
      catch (Exception ex)
      {
        string errorMessage = "خطا در بازگردانی موجودیت : " + ex.Message;

        if (ex.InnerException != null)
        {
          errorMessage += " | Inner Error: " + ex.InnerException.Message;
        }

        var response = new ServerResponse<bool>(false, errorMessage, false);

        return response;
      }
    }

    #endregion
  }
}
