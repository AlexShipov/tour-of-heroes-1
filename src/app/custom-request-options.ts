import { BaseRequestOptions, RequestOptions, RequestOptionsArgs } from '@angular/http';
import { resolve } from 'url';

export class CustomRequestOptions extends BaseRequestOptions {
    public merge(options?: RequestOptionsArgs): RequestOptions {
        if (options && options.url) {
            options.url = resolve(API_URL, options.url);
        }
        let result = super.merge(options);
        result.merge = this.merge;
        return result;
    }
}
