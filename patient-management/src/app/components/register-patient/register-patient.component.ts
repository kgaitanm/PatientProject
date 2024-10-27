import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/patient.model';
import { MatFormFieldModule } from '@angular/material/form-field';  
import { MatInputModule } from '@angular/material/input'; 
import { FormsModule } from '@angular/forms';

import { MatButtonModule } from '@angular/material/button';

import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';


@Component({
  selector: 'app-register-patient',
  templateUrl: './register-patient.component.html',
  imports: [MatFormFieldModule, MatInputModule,FormsModule,MatButtonModule,MatCardModule,MatGridListModule],
  standalone: true,
  styleUrl: './register-patient.component.scss'
})
export class RegisterPatientComponent {
  patient: Patient = {
    firstName: '',
    lastName: '',
    dateOfBirth: new Date(),
    address: '',
    email: '',
    phoneNumber: '',
    socialInsuranceNumber: '',
  };

  constructor(private patientService: PatientService, private router: Router) {}

  registerPatient(): void {
    this.patientService.registerPatient(this.patient).subscribe(() => {
      this.router.navigate(['/']);
    });
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}
