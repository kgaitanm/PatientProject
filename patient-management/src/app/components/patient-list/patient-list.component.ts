import { Component, OnInit,ChangeDetectorRef  } from '@angular/core';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/patient.model';
import { Router } from '@angular/router';

import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatFormFieldModule, MatInputModule,MatIconModule,MatToolbarModule,MatCardModule],
  providers: [PatientService],
})
export class PatientListComponent implements OnInit {
  patients: Patient[] = [];

  dataSource = new MatTableDataSource<Patient>(); 
  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'actions'];

  constructor(private patientService: PatientService, private router: Router, private cd: ChangeDetectorRef) {

    
  }

  ngOnInit(): void {
    this.loadPatients();
  }

  goToRegister(): void {
    this.router.navigate(['/register-patient']);
  }

  loadPatients(): void {
    this.patientService.getPatients().subscribe((data) => {
      this.dataSource.data = data;
    });
  }

  viewPatient(id: string): void {
    this.router.navigate(['/patient-details', id]);
  }

  editPatient(id: string): void {
    this.router.navigate(['/edit-patient', id]);
  }

  deletePatient(id: string): void {
    this.patientService.deletePatient(id).subscribe(() => {
      this.loadPatients();
    });
  }
}
