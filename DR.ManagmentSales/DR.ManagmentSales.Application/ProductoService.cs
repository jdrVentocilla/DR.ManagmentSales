﻿using Core;
using Core.GestionDeExcepciones;
using Core.Services;
using DR.ManagmentSales.Domain;
using DR.ManagmentSales.Infrastructure.Interface;

namespace DR.ManagmentSales.Application
{
    public class ProductoService : IServicio<Producto>
    {
        private IManagmentSalesUOW _managmentSalesUOW;
        public ProductoService(IManagmentSalesUOW managmentSalesUOW)
        {
            _managmentSalesUOW = managmentSalesUOW;
        }
        public async Task<StateExecution> UpsertAsync(Producto entidad, CancellationToken cancellationToken)
        {
            
            Producto entidadEncontrado = this._managmentSalesUOW._productoRepository.Find(x=> x.Id == entidad.Id);

            if (entidadEncontrado == null)
            {
                this._managmentSalesUOW._productoRepository.Add(entidad);
            }
            else
            {
                this._managmentSalesUOW._productoRepository.Update(entidad);
            }

            
            await this._managmentSalesUOW.SaveChangesAsync(cancellationToken);

            return (new StateExecution()
            {

                Status = true,
                StateType = State.Ok,
                MessageState = new Message() { Description = "Registro modificado con éxito." },

            });
        }

        public async Task<StateExecution> AddAsync(Producto entidad, CancellationToken cancellationToken)
        {
            this._managmentSalesUOW._productoRepository.Add(entidad);
            await this._managmentSalesUOW.SaveChangesAsync(cancellationToken);

            return (new StateExecution()
            {


                Status = true,
                StateType = State.Ok,
                MessageState = new Message() { Description = "Registro guardado con éxito." },


            });
        }

        public async Task<StateExecution> DeleteAsync(string id, CancellationToken cancellationToken)
        {

            Producto entidad = this._managmentSalesUOW._productoRepository.Find(x => x.Id == id);

            if (entidad == null)
            {
                return new StateExecution()
                {
                    Status = false,
                    StateType = State.ErrorNotContent,
                    MessageState = new Message() { Description = "Registro no encontrado." },

                };
            }
            else
            {
                this._managmentSalesUOW._productoRepository.Delete(entidad);
                await this._managmentSalesUOW.SaveChangesAsync(cancellationToken);
            }




            return new StateExecution()
            {

                Status = true,
                StateType = State.Ok,
                MessageState = new Message() { Description = "Registro eliminado con éxito." },

            };
        }

        public async Task<StateExecution<Producto>> FindAsync(string id, CancellationToken cancellationToken)
        {
            Producto entidad = this._managmentSalesUOW._productoRepository.Find(x => x.Id == id);

            return new StateExecution<Producto>()
            {
                Status = true,
                StateType = State.Ok,
                MessageState = new Message() { Description = "Registro encontrado." },
                Data = entidad

            };
        }

        public async Task<StateExecution<IEnumerable<Producto>>> GetAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Producto> ListaEntidad = this._managmentSalesUOW._productoRepository.Get();

            return new StateExecution<IEnumerable<Producto>>()
            {

                Status = true,
                StateType = State.Ok,
                MessageState = new Message() { Description = "Registros encontrados." },
                Data = ListaEntidad
            };
        }
    }
}
