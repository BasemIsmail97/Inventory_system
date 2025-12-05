

namespace Services.Implementations
{
    public class SalesOrderService(IUnitOfWork unitOfWork ,IMapper mapper) : ISalesOrderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<int> CreateSalesOrderAsync(CreateOrUpdateSalesOrderDto createSalesOrderDto)
        {
            createSalesOrderDto.CreatedAt = DateTime.Now;
            var salesOrder = _mapper.Map<SalesOrder>(createSalesOrderDto);
           await _unitOfWork.GetRepository<SalesOrder>().AddAsync(salesOrder);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteSalesOrderAsync(int SalesOrderId)
        {
            var Repo =   _unitOfWork.GetRepository<SalesOrder>();
            var salesOrder =  await Repo.GetByIdAsync(SalesOrderId);
            if(salesOrder is null)
            {
                return false;
            }
            Repo.Delete(salesOrder);
            await  _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PagenatedResult<SalesOrderDto>> GetAllSalesOrderAsync(SalesOrderSpecificationParameters specificationParameters)
        {
            var Repo = _unitOfWork.GetRepository<SalesOrder>();
            var spec = new SalesOrderWithCustomerAndUserAndSalesOrdersDetailsSpecification(specificationParameters);
            var SalesOrders = await Repo.GetAllAsync(spec);
            var SalesOrderDtos = _mapper.Map<IEnumerable<SalesOrderDto>>(SalesOrders);
            var pageSize = SalesOrderDtos.Count();
            var countSpecification = new SalesOrderCountSpecification(specificationParameters);
            var TotalCount = await Repo.CountAsync(countSpecification);
            return new PagenatedResult<SalesOrderDto>(specificationParameters.PageIndex, pageSize, TotalCount, SalesOrderDtos);
        }

        public async Task<SalesDetailsDto> GetSalesOrderByIdAsync(int SalesOrderId)
        {
            var Spec = new SalesOrderWithCustomerAndUserAndSalesOrdersDetailsSpecification(SalesOrderId);
            var salesOrder =  await _unitOfWork.GetRepository<SalesOrder>().GetByIdAsync(Spec);
            if (salesOrder is null)
            {
                throw new KeyNotFoundException($"Sales Order with id {SalesOrderId} not found.");
            }
            var salesOrderDto = _mapper.Map<SalesDetailsDto>(salesOrder);
            return salesOrderDto;
        }

        public async Task<int> UpdateSalesOrderAsync(CreateOrUpdateSalesOrderDto updateSaleseOrderDto)
        {
           var existingSalesOrder = await _unitOfWork.GetRepository<SalesOrder>().GetByIdAsync(updateSaleseOrderDto.Id);
            if(existingSalesOrder is null)
            {
                throw new KeyNotFoundException($"Sales Order with id {updateSaleseOrderDto.Id} not found.");
            }
            updateSaleseOrderDto.UpdatedAt = DateTime.Now;
            var UpdatedSalesOrder = _mapper.Map<SalesOrder>(updateSaleseOrderDto);
            _unitOfWork.GetRepository<SalesOrder>().Update(UpdatedSalesOrder);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
