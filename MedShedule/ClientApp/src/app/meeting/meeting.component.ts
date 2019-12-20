import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

import { Meeting } from '../../_models/Meeting';

import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { MeetingService } from '../../_services/meeting.service';
import { BsLocaleService } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';

import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { HttpErrorResponse } from '@angular/common/http';
defineLocale('pt-br', ptBrLocale);


@Component({
  selector: 'app-meeting',
  templateUrl: './meeting.component.html',
  styleUrls: ['./meeting.component.css']
})
export class MeetingComponent implements OnInit {

  meetings: Meeting[];
  meeting: Meeting;
  _listFilter: string;
  meetingsFiltered: Meeting[];
  registerForm: FormGroup;
  modeSave: string;
  messageDeleteMeetingConfirm: string;

  get listFilter(): string {
    return this._listFilter;
  }

  set listFilter(value: string) {
    this._listFilter = value;
    this.meetingsFiltered = this.listFilter ? this.meetingFilter(this.listFilter) : this.meetings;
  }


  constructor(
    private meetingService: MeetingService,
    private modalService: BsModalService,
    private formBuilder: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService
  ) {
    this.localeService.use('pt-br');
  }

  ngOnInit() {
    this.validation();
    this.getMeetings();
  }

  meetingFilter(filterBy: string): Meeting[] {
    filterBy = filterBy.toLowerCase();
    return this.meetings.filter(meeting => meeting.patientName.toLowerCase().indexOf(filterBy) !== -1);
  }

  getMeetings() {
    this.meetingService.getAllMeeting().subscribe(
      (_meetings: Meeting[]) => {
        this.meetings = _meetings;
        this.meetingsFiltered = this.meetings;
        console.log(_meetings);
      }, error => { console.error(error); });
  }


  editMeeting(meeting: Meeting, template: any) {
    this.modeSave = "put";
    this.openModal(template);
    this.meeting = meeting;
    this.registerForm.patchValue(meeting);
  }

  newMeeting(template: any) {
    this.modeSave = "post";
    this.openModal(template);
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  validation() {
    this.registerForm = this.formBuilder.group({
      patientName: ['', [Validators.required]],
      beginMeeting: ['', [Validators.required]],
      endMeeting: ['', [Validators.required]],
      birth: ['', [Validators.required]],
      observation: ['']
    });
  }



  save(template: any) {
    if (this.registerForm.valid) {

      if (this.modeSave === "post") {
        this.meeting = Object.assign({}, this.registerForm.value);
        this.meetingService.postMeeting(this.meeting).subscribe(
          (newMeeting: Meeting) => {
            console.log(newMeeting);
            template.hide();
            this.getMeetings();
            this.toastr.success('Item saved');
          }, error => {
            if (error.status === 400) {
              if (error instanceof HttpErrorResponse) {
                const validationError = error.error;
                Object.keys(validationError).forEach(prop => {
                  const formControl = this.registerForm.get(prop);
                  if (formControl) {
                    formControl.setErrors({
                      serverError: validationError[prop]
                    });
                  }
                });
              }
              this.toastr.warning("Correct the errors.");
            }
            else {
              this.toastr.error('Error');
            }
            console.error(error);
          });

      }
      else {
        this.meeting = Object.assign({ id: this.meeting.id }, this.registerForm.value);
        this.meetingService.putMeeting(this.meeting).subscribe(
          () => {
            template.hide();
            this.getMeetings();
            this.toastr.success('Item saved');
          }, error => {

            if (error.status === 400) {
              if (error instanceof HttpErrorResponse) {
                const validationError = error.error;
                Object.keys(validationError).forEach(prop => {
                  const formControl = this.registerForm.get(prop);
                  if (formControl) {
                    formControl.setErrors({
                      serverError: validationError[prop]
                    });
                  }
                });
              }
              this.toastr.warning("Correct the errors.");
            }
            else {
              this.toastr.error('Error');
            }
            console.error(error);
          });
      }
    }
  }

  deleteMeeting(meeting: Meeting, template: any) {
    this.openModal(template);
    this.meeting = meeting;
    this.messageDeleteMeetingConfirm = "Confirm meeting delete: " + meeting.patientName;
  }

  confirmDelete(template: any) {
    this.meetingService.deleteMeeting(this.meeting.id).subscribe(
      () => {
        template.hide();
        this.getMeetings();
        this.toastr.success('Item removed');
      }, error => {
        this.toastr.error('Error');
        console.log(error);
      }
    );
  }
 
}
