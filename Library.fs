namespace Fake.Azure.Cli

(*
Task.json:
        {
            "name": "connectedServiceNameARM",
            "aliases": [
                "azureSubscription"
            ],
            "type": "connectedService:AzureRM",
            "label": "Azure subscription",
            "required": true,
            "helpMarkDown": "Select an Azure resource manager subscription for the deployment"
        },

https://github.com/Microsoft/azure-pipelines-tasks/blob/master/Tasks/AzureCLIV1/azureclitask.ts

        var servicePrincipalId: string = tl.getEndpointAuthorizationParameter(connectedService, "serviceprincipalid", false);
        let authType: string = tl.getEndpointAuthorizationParameter(connectedService, 'authenticationType', true);
        let cliPassword: string = null;
        if (authType == "spnCertificate") {
            tl.debug('certificate based endpoint');
            let certificateContent: string = tl.getEndpointAuthorizationParameter(connectedService, "servicePrincipalCertificate", false);
            cliPassword = path.join(tl.getVariable('Agent.TempDirectory') || tl.getVariable('system.DefaultWorkingDirectory'), 'spnCert.pem');
            fs.writeFileSync(cliPassword, certificateContent);
            this.cliPasswordPath = cliPassword;

        }
        else {
            tl.debug('key based endpoint');
            cliPassword = tl.getEndpointAuthorizationParameter(connectedService, "serviceprincipalkey", false);
        }

        var tenantId: string = tl.getEndpointAuthorizationParameter(connectedService, "tenantid", false);
        var subscriptionID: string = tl.getEndpointDataParameter(connectedService, "SubscriptionID", true);

        // set az cli config dir
        this.setConfigDirectory();

        //login using svn
        this.throwIfError(tl.execSync("az", "login --service-principal -u \"" + servicePrincipalId + "\" -p \"" + cliPassword + "\" --tenant \"" + tenantId + "\""), tl.loc("LoginFailed"));
        this.isLoggedIn = true;
        //set the subscription imported to the current subscription
        this.throwIfError(tl.execSync("az", "account set --subscription \"" + subscriptionID + "\""), tl.loc("ErrorInSettingUpSubscription"));



https://github.com/Microsoft/azure-pipelines-task-lib/blob/350e635901a8a657523458213a0426a8d136bcdc/node/task.ts#L351
/**
 * Gets the endpoint authorization parameter value for a service endpoint with specified key
 * If the endpoint authorization parameter is not set and is not optional, it will throw.
 *
 * @param id name of the service endpoint
 * @param key key to find the endpoint authorization parameter
 * @param optional optional whether the endpoint authorization scheme is optional
 * @returns {string} value of the endpoint authorization parameter value
 */
export function getEndpointAuthorizationParameter(id: string, key: string, optional: boolean): string {
    var authParam = im._vault.retrieveSecret('ENDPOINT_AUTH_PARAMETER_' + id + '_' + key.toUpperCase());

    if (!optional && !authParam) {
        throw new Error(loc('LIB_EndpointAuthNotExist', id));
    }

    debug(id + ' auth param ' + key + ' = ' + authParam);
    return authParam;
}





/**
 * Interface for EndpointAuthorization
 * Contains a schema and a string/string dictionary of auth data
 */
export interface EndpointAuthorization {
    /** dictionary of auth data */
    parameters: {
        [key: string]: string;
    };

    /** auth scheme such as OAuth or username/password etc... */
    scheme: string;
}

/**
 * Gets the authorization details for a service endpoint
 * If the authorization was not set and is not optional, it will throw.
 * 
 * @param     id        name of the service endpoint
 * @param     optional  whether the url is optional
 * @returns   string
 */
export function getEndpointAuthorization(id: string, optional: boolean): EndpointAuthorization {
    var aval = im._vault.retrieveSecret('ENDPOINT_AUTH_' + id);

    if (!optional && !aval) {
        setResult(TaskResult.Failed, loc('LIB_EndpointAuthNotExist', id));
    }

    console.log(id + ' exists ' + (aval !== null));
    debug(id + ' exists ' + (aval !== null));

    var auth: EndpointAuthorization;
    try {
        auth = <EndpointAuthorization>JSON.parse(aval);
    }
    catch (err) {
        throw new Error(loc('LIB_InvalidEndpointAuth', aval));
    }

    return auth;
}


*)
module Say =
    let hello name =
        printfn "Hello %s" name
