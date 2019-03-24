import {Injectable} from '@angular/core';
import {RippleGlobalOptions} from '@angular/material';

@Injectable({providedIn: 'root'})
export class RippleOptions implements RippleGlobalOptions {
    disabled = false;
}
