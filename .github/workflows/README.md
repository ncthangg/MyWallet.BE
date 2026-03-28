# Release and Deployment Workflow

## Purpose
Automate Docker image build and deployment for staging and production from Git tags.

## Environments
- Staging: https://staging.be.cocoqr.cocome.online
- Production: https://api.cocoqr.cocome.online

## Branch and Tag Strategy
Deployment targets and release types are strictly controlled by the tag pattern and its source branch.

| Tag Pattern | Source Branch | Environment | GitHub Release |
|-------------|---------------|-------------|----------------|
| `v*.*.*-rc*` | `dev` | Staging | None |
| `v*.*.*` | `main` | Production | Full Release |
| `v*.*.*-pre` | `main` | Production | Pre-release |
| `v*.*.*-no-release` | `main` | Production | None |

Note: Production releases (`v*.*.*`) will automatically fail if no corresponding `-rc*` tag is found in the commit history.

## Deployment Flow
1. Build job runs on every valid tag:
	- validate tag format
	- validate source branch history
	- build and push Docker image
2. Staging path:
	- only when channel is staging (rc tags)
	- deploy to STAGING server via SSH
3. Production path:
	- when channel is production
	- for vX.Y.Z release, at least one matching RC tag must already exist
	- deploy to PRODUCTION server via SSH
4. GitHub Release creation:
	- only for production when release_type is not none
	- vX.Y.Z-pre is marked as prerelease

## How to Trigger Deployments
```bash
# Staging deploy (from dev history)
git tag v1.2.3-rc1
git push origin v1.2.3-rc1

# Production pre-release deploy (from main history)
git tag v1.2.3-pre
git push origin v1.2.3-pre

# Production release deploy + GitHub Release (from main history)
git tag v1.2.3
git push origin v1.2.3

# Production deploy only, no GitHub Release (from main history)
git tag v1.2.3-no-release
git push origin v1.2.3-no-release
```

## Docker Image
```text
dockerhub/cocoqr-be.X.X
```

Built and pushed as:
```text
<DOCKER_USERNAME>/cocoqr-be:<tag>
```

## Rollback
```bash
git tag v1.0.0
git push origin v1.0.0
```
