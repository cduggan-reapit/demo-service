# Stuff to do
- ~~scopes~~
- ~~error handling~~
- unit testing api
- dbcontextfactory rather than dbcontext (can this be standardised?  we've got PlatformDbFactory elsewhere)
  - alternative integration tests w/dynamo (although maybe not? we'd need a fake dynamo provider)
- concurrency (etags)
- neo-reapitconnect identity provider service (who did thing)
- github workflows
  - commitlinting
  - build + test
  - deployment?  is that too far for the template?
    - We'll need to decide what we're encouraging here (feature branches w/rollback using github releases vs. trunk based w/fail forward using a release branch)

# Pain points
- admittedly a lot of code out of the gate
- separation of concerns by example, somewhat forced by project structure, but could still be broken
