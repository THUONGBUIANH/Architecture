export class User {
  id: string;
  userName: string;
  password: string;
  email: string;
  birthDay: Date;
  isDeleted: number;

  constructor(init: User) {
    this.id = init.id;
    this.userName = init.userName;
    this.email = init.email;
    this.isDeleted = init.isDeleted;
    this.birthDay = new Date(init.birthDay);
  }
}
