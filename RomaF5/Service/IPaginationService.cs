using RomaF5.Models;

namespace RomaF5.Service
{
    public interface IPaginationService
    {
        PginationModel GetPagination(int pageNumber, int pageSize, int totalItems);
    }

    public class PaginationService : IPaginationService
    {
        public PginationModel GetPagination(int pageNumber, int pageSize, int totalItems)
        {
            var paginationModel = new PginationModel();

            // Calcula el número total de páginas
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Calcula el número de elementos en la página actual
            var itemsInPage = totalItems < pageNumber * pageSize ? totalItems : pageNumber * pageSize;

            // Calcula el número de elementos en la página anterior
            var itemsInPreviousPage = (pageNumber - 1) * pageSize;

            // Calcula el número de elementos en la página siguiente
            var itemsInNextPage = pageNumber * pageSize + pageSize;

            // Asigna los valores a la modelo de paginación
            paginationModel.PageNumber = pageNumber;
            paginationModel.PageSize = pageSize;
            paginationModel.TotalItems = totalItems;
            paginationModel.TotalPages = totalPages;
            paginationModel.HasPreviousPage = pageNumber > 1;
            paginationModel.HasNextPage = pageNumber < totalPages;
            paginationModel.ItemsInPage = itemsInPage;
            paginationModel.ItemsInPreviousPage = itemsInPreviousPage;
            paginationModel.ItemsInNextPage = itemsInNextPage;

            return paginationModel;
        }
    }
}
