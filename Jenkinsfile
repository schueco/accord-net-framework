@Library("Jenkins-Library") _
pipeline
{
    parameters
    {
        booleanParam defaultValue: true, name: 'cleanUp', description: 'Clean up the build directory after the build is completed.'
    }
    agent
    {
        label 'vs_2022'
    }
    options 
    {
        timeout(time:30, unit:'MINUTES')
    }
    environment
    {
        CONFIGURATION = 'Release'
        PLATFORM = 'Any CPU'
    }
    stages
    {
        stage('Prepare Build')
        {
            steps
            {
                dir("${env.WORKSPACE}")
                {
                    script
                    {
                        env.IS_TRIGGERED_BY_USER = !currentBuild.getBuildCauses('hudson.model.Cause$UserIdCause').isEmpty()

                        env.MULTIBRANCH_PIPELINE_NAME = currentBuild.fullProjectName.split('/')[0]
                        env.ARTIFACTORY_BUILD_NAME = "${env.MULTIBRANCH_PIPELINE_NAME}-${env.BRANCH_NAME}"
                    }
                }
            }
        }
        stage('Calculate NuGET version')
        {
            steps
            {
                dir("${env.WORKSPACE}")
                {
                    script
                    {
                        env.NUGET_VERSION = bat(returnStdout:true,
                                                    script: "@python.exe calculateNuGETVersion.py ${BRANCH_NAME} $P4_CHANGELIST"
                                                )
                    }
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
        // stage('Build net6.0')
        // {
        //     steps
        //     {
        //         dir("${env.WORKSPACE}\\Sources")
        //         {
        //             bat """
        //             dotnet build --no-restore --framework net6.0 --configuration ${env.CONFIGURATION} -p:Platform="${env.PLATFORM}"
        //             """
        //         }
        //     }
        // }
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
                        files = findFiles( glob: '*.nuspec' )
                        files.each
                        {
                            f ->
                                bat script: "nuget pack ${f.path} -Version ${env.NUGET_VERSION}"
                        }

                        jfrogCliUpload(JFROG: 'jf', FILE: '*.nupkg', TARGET: "nuget-local/Accord.NET/${NUGET_VERSION}/", ARTIFACTORY_BUILD_NAME: env.ARTIFACTORY_BUILD_NAME, ARTIFACTORY_BUILD_NUMBER: env.BUILD_NUMBER, FLAT: true)
                        jfrogCliCollectEnvVar(JFROG: 'jf', ARTIFACTORY_BUILD_NAME: env.ARTIFACTORY_BUILD_NAME, ARTIFACTORY_BUILD_NUMBER: env.BUILD_NUMBER)
                        jfrogCliPublishInfo(JFROG: 'jf', ARTIFACTORY_BUILD_NAME: env.ARTIFACTORY_BUILD_NAME, ARTIFACTORY_BUILD_NUMBER: env.BUILD_NUMBER)
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