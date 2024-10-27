import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/patient.model';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';  
import { CommonModule } from '@angular/common'; 
import { Observable } from 'rxjs';

@Component({
  selector: 'app-patient-details',
  standalone: true,
  imports: [MatCardModule,CommonModule,MatButtonModule],
  templateUrl: './patient-details.component.html',
  styleUrl: './patient-details.component.scss'
})
export class PatientDetailsComponent implements OnInit {
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

  goBack(): void {
    this.router.navigate(['/']);
  }
}