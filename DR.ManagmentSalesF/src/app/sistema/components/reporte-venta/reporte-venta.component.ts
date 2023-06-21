import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { formatearFecha } from 'src/app/core/helpers/date.helpers';
import { TituloCabeceraService } from 'src/app/core/services/titulo.service';
import { ProblemDetails } from 'src/app/shared/models/problem-details.model';
import { SucessResponse } from 'src/app/shared/models/succes-response.model';
import { SharedModalService } from 'src/app/shared/services/shared-modal.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { AsesorSummary } from '../../models/asesor-summary.model';
import { Venta } from '../../models/venta.model';
import { VentaService } from '../../services/venta.service';

@Component({
  selector: 'app-reporte-venta',
  templateUrl: './reporte-venta.component.html',
  styleUrls: ['./reporte-venta.component.css']
})
export class ReporteVentaComponent implements OnInit {


  detalles : Venta[];
  grupos : AsesorSummary[];
  fechaInicial : Date;
  fechaFinal : Date;
  public valorMinimoDeOtros = 1;

  constructor(private ventaService : VentaService,
              private tituloCabeceraService: TituloCabeceraService,
              private sharedModalService: SharedModalService,
              private spinner: SpinnerService,
              private dialog: MatDialog) {


                this.tituloCabeceraService.setearTituloActual("CONSOLIDADO DE VENTAS");


              }

  ngOnInit(): void {

      this.fechaInicial = new Date(2022,1,1);
      this.fechaFinal = new Date(2023,12,31);
      this.loadReport();

  }

  loadReport(){

    this.spinner.showBallAtom("report-venta");

    this.ventaService.report(formatearFecha(this.fechaInicial.toDateString()),formatearFecha(this.fechaFinal.toDateString()))
      .subscribe((response: SucessResponse) => {

        this.detalles = response.data.detalle;
        this.grupos = response.data.grupos;

        this.spinner.hide("report-venta");
      }
        , (error: ProblemDetails) => {
          this.spinner.hide("report-venta");
          try {
            this.sharedModalService.mostrarMessageModal({description: error.title , detail :error.detail }, false);
          } catch (e) {
            this.sharedModalService.mostrarMessageModal( {description :'Error al realizar la operación, intente recargar la página'}, false);
          }

        });

  }

}
