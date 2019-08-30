import { Component, OnInit } from '@angular/core';
import { User, EditUser } from '../../_models';
import { AuthenticationService } from '../../_services';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IMyDpOptions } from 'mydatepicker';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  users: User[];
  editUser: EditUser = new EditUser();
  editForm: FormGroup;
  loading = false;
  displayEditModal = 'none';
  submitted = false;
  myDatePickerOptions: IMyDpOptions = {
    // other options...
    dateFormat: 'dd/mm/yyyy'
  };
  error = '';
  id: string;

  constructor(
    private authenticationService: AuthenticationService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.getUsers();
  }

  private getUsers() {
    this.authenticationService.getUsers().subscribe(res => {
      this.users = res;
    });
  }

  onOpenEditModal() {
    this.displayEditModal = 'block';
  }

  onCloseEditModal() {
    this.displayEditModal = 'none';
  }

  // convenience getter for easy access to form fields
  get f(): any {
    return this.editForm.controls;
  }

  edit(user: User) {
    this.id = user.id;
    // Regiter reactive form edit user
    this.editForm = this.formBuilder.group({
      birthday: [this.setByDate(user.birthDay), Validators.required],
      email: [user.email, [Validators.required, Validators.email]]
    });

    this.onOpenEditModal();
  }

  private setByDate(date: Date) {
    return {
      date: {
        year: date.getFullYear(),
        month: date.getMonth() + 1,
        day: date.getDate()
      }
    };
  }

  private getStringDateByObjectDate(objDate: any) {
    const strDate: string =
      objDate.year + '/' + objDate.month + '/' + objDate.day;
    return strDate;
  }

  active(id: string) {
    if (confirm('You sure!!!')) {
      this.authenticationService.active(id).subscribe(
        res => {
          alert('Successful!');
          this.getUsers();
        }
      );
    }
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    if (this.editForm.invalid || this.editForm.errors) {
      this.loading = false;
      return;
    }

    this.editUser.email = this.editForm.controls.email.value;
    this.editUser.birthDay = this.getStringDateByObjectDate(
      this.editForm.controls.birthday.value.date
    );

    this.authenticationService.editUser(this.id, this.editUser).subscribe(
      res => {
        this.getUsers();
        this.onCloseEditModal();
        this.loading = false;
        alert('Successful!');
      },
      err => {
        this.error = err;
        this.loading = false;
      }
    );
  }
}
