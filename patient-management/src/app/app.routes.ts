import { Routes } from '@angular/router';
import { PatientListComponent } from './components/patient-list/patient-list.component';
import { RegisterPatientComponent } from './components/register-patient/register-patient.component';
import { EditPatientComponent } from './components/edit-patient/edit-patient.component';
import { PatientDetailsComponent } from './components/patient-details/patient-details.component';

export const routes: Routes = [
  { path: '', component: PatientListComponent },
  { path: 'register-patient', component: RegisterPatientComponent },
  { path: 'edit-patient/:id', component: EditPatientComponent },
  { path: 'patient-details/:id', component: PatientDetailsComponent },
];