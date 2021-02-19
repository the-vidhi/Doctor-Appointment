using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class ViewModel
    {
        public int Medi_Id { get; set; }   
        public Medicine medicine { get; set; }
        public List<Medicine> medicineList { get; set; }

        public ProductReview productReviewData { get; set; }        
        public List<ProductReview> ProductReviewList { get; set; }

        public Review review { get; set; }
        public List<Review> reviewList { get; set; }

        public List<Doctor> doctorList { get; set; }
        public Doctor doctor { get; set; }

        public Patient patient { get; set; }
        public List<Patient> patientList { get; set; }
        public Clinic clinic { get; set; }
        
        public Bill bill { get; set; }
        public List<Bill> billList { get; set; }

        public Cart cart { get; set; }
        public List<Cart> cartList { get; set; }
        

        public Order order { get; set; }
        public List<Order> orderList { get; set; }

        public OrderMedical orderMedical { get; set; }
        public List<OrderMedical> orderMedicalList { get; set; }


        public State state { get; set; }
        public City city { get; set; }
        

        public Appointment appointment { get; set; }
        public List<Appointment> appointmentList { get; set; }

        public Payment payment { get; set; }
        public List<Payment> paymentList { get; set; }
        public Career career { get; set; }

        public Category category { get; set; }
        public List<Category> categoryList { get; set; }

        public List<Cart, Medicine> cartmedList { get; set; }

        public class List<Cart, Medicine> 
        {
            
        }
    }
}
