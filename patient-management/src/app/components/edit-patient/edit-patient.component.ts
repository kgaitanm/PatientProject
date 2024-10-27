import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { FormsModule } from '@angular/forms';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/patient.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-edit-patient',
  templateUrl: './edit-patient.component.html',
  styleUrl:'./edit-patient.component.scss',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    FormsModule
  ],
})
export class EditPatientComponent implements OnInit {
  patient$!: Observable<Patient>;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private patientService: PatientService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.patient$ = this.patientService.getPatient(id);
    }
  }

  savePatient(patient: Patient): void {
    this.patientService.updatePatient(patient).subscribe(() => {
      this.router.navigate(['/']);
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0]; 
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}