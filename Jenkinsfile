@Library('global-shared-library@main') _

pipeline
{
    parameters
    {
        booleanParam defaultValue: true, name: 'cleanUp', description: 'Clean up the build directory after the build is completed.'
    }
    agent
    {
        label vmAgent()
    }
    options
    {
        timeout(time:30, unit:'MINUTES')
    }
    environment
    {
        CONFIGURATION = 'Release'
        PLATFORM = 'Any CPU'
        JFROG_CLI_BUILD_NAME = JOB_NAME.replaceAll( '.*?/(.*)', '$1' ).toLowerCase()
        JFROG_CLI_BUILD_NUMBER = "${BUILD_NUMBER}"
    }
    stages
    {
        stage('Calculate NuGET version')
        {
            steps
            {
                script
                {
                    env.NUGET_VERSION = bat(returnStdout:true,
                                                script: "@python.exe calculateNuGETVersion.py ${BRANCH_NAME}"
                                            )
                }
            }
        }
        stage('NuGet restore')
        {
            steps
            {
                dir("${env.WORKSPACE}\\Sources")
                {
                    bat """
                        jf nuget restore \".\\Accord.NET (NETStandard).sln\"
                    """
                }
            }
        }

        stage('Security audit') {
            steps {
                securityAudit {
                }
            }
        }

        stage('Collect build info') {
            steps {
                collectBuildinfo {
                }
            }
        }

        stage('Build net6.0')
        {
            steps
            {
                dir("${env.WORKSPACE}\\Sources")
                {
                    bat """
                        dotnet build --no-restore --framework net6.0 --configuration ${env.CONFIGURATION} -p:Platform="${env.PLATFORM}"
                    """
                }
            }
        }
        stage('Build net8.0')
        {
            steps
            {
                dir("${env.WORKSPACE}\\Sources")
                {
                    bat """
                       dotnet build --no-restore --framework net8.0 --configuration ${env.CONFIGURATION} -p:Platform="${env.PLATFORM}"
                    """
                }
            }
        }
        stage('Tests')
        {
            steps
            {
                dir("${env.WORKSPACE}\\Sources")
                {
                    bat """
                        dotnet test --no-build --logger trx --results-directory test_results --configuration ${env.CONFIGURATION} --framework net8.0 /p:Platform="${env.PLATFORM}"
                    """
                }
            }
        }
        stage('NuGet package')
        {
            steps
            {
                dir( "${env.WORKSPACE}\\Setup\\NuGet" )
                {
                   script
                   {
                        def files = findFiles( glob: '*.nuspec' )
                        files.each
                        {
                            f ->
                                bat script: "nuget pack ${f.path} -Version ${env.NUGET_VERSION}"
                        }

                        uploadNuget {
                            FILE_OR_PATTERN: "*.nupkg"
                            UPLOAD_PATH = "Accord.NET"
                            SIGN_NUGET = true
                        }
                        publishBuildinfo {
                        }
                    }
                }
            }
        }
    }
    post
    {
        always
        {
            script
            {
                if (params.cleanUp) {
                    cleanWs cleanWhenNotBuilt : false
                }
                emailext (
                    body: '${SCRIPT, template="groovy-html.template"}',
                    attachLog: true,
                    recipientProviders: [developers()],
                    subject: '$BUILD_STATUS Build in Accord.NET Framework build: $PROJECT_NAME - #$BUILD_NUMBER - Branch ${BRANCH_NAME}'
                )
            }
        }
        failure
        {
            sendMail(BRANCH_NAME: "${BRANCH_NAME}")
        }
    }
}