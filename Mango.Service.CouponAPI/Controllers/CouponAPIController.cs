using AutoMapper;
using Mango.Service.CouponAPI.Data;
using Mango.Service.CouponAPI.Models;
using Mango.Service.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Mango.Service.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private ResponseDto _response;
        private readonly AddDbContext _db;
        private IMapper _mapper;

        public CouponAPIController(AddDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList(); // return  (many object ) list
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")] //route 
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.CouponId == id); // return spicefic object
                _response.Result = _mapper.Map<CouponDto>(obj);
                //CouponDto couponDto = new CouponDto()
                //{
                //    CouponId = obj.CouponId,
                //    CouponCode = obj.CouponCode,
                //    DiscountAmount = obj.DiscountAmount,
                //    MinAmount = obj.MinAmount
                //};

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }



        [HttpGet]
        [Route("GetByCode/{code}")] //route 
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon obj = _db.Coupons.FirstOrDefault(u => u.CouponCode.ToLower() == code.ToLower()); // return spicefic object
                if (obj == null) 
                {
                    _response.IsSuccess=false;
                
                }
                _response.Result = _mapper.Map<CouponDto>(obj);

            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }




        [HttpPost]
        [Route("GetByCode/{code}")] //route 
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(obj);
                _db.SaveChanges();

              
                _response.Result = _mapper.Map<CouponDto>(obj);

            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }




        [HttpPut]
        [Route("GetByCode/{code}")] //route 
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(obj);
                _db.SaveChanges();


                _response.Result = _mapper.Map<CouponDto>(obj);

            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }




        [HttpDelete]
        [Route("GetByCode/{code}")] //route 
        public ResponseDto Delete(int id )
        {
            try
            {
                Coupon obj = _db.Coupons.First(u=>u.CouponId==id);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
