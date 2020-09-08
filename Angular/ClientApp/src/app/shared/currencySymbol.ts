import { Pipe, PipeTransform } from '@angular/core';
import { getCurrencySymbol } from '@angular/common';

@Pipe({
  name: 'currencySymbol'
})
export class CurrencySymbolPipe implements PipeTransform {
  transform(currencyCode: string, format: 'wide' | 'narrow' = 'narrow', locale?: string): any {
    return getCurrencySymbol(currencyCode, format, locale);
  }
}