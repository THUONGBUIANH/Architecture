import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './_services';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'app-root',
    templateUrl: 'app.component.html'
})
export class AppComponent implements OnDestroy {
    private unsubscribe: Subject<void> = new Subject();
    isToken: boolean;

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        this.authenticationService.currentToken
            .pipe(takeUntil(this.unsubscribe))
            .subscribe(res => this.isToken = res !== null);
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }

    ngOnDestroy() {
        console.log('ngOnDestory');
        this.unsubscribe.next();
        this.unsubscribe.complete();
    }
}
